using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using ZhonTai.Admin.Contracts.Domain.Sso;
using ZhonTai.Admin.Contracts.Services.Sso;
using ZhonTai.Admin.Contracts.Services.Sso.Dto;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Resources;
using ZhonTai.Common.Helpers;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;

namespace ZhonTai.Admin.Services.Sso;

/// <summary>
/// 单点登录服务（一次性 Ticket 授权码方案）
/// </summary>
[Order(96)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class SsoService : BaseService, ISsoService, IDynamicApi
{
    private readonly AppConfig _appConfig;
    private readonly ISsoAppManageRepository _ssoAppRep;
    private readonly ISsoLogRepository _ssoLogRep;
    private readonly AdminLocalizer _adminLocalizer;

    // 流程：
    //   1. 已登录用户点击"单点登录"按钮 -> GenerateTicketAsync 生成 Ticket（存缓存，默认5分钟）
    //   2. 前端拼接到第三方回调地址后跳转（URL仅携带乱码GUID，JWT不外露）
    //   3. 第三方后端用 app_secret 对 (AppId+Ticket+Timestamp) 签名，调用 ValidateTicketAsync 换取用户信息
    //   4. Ticket 一次性消费，校验通过后即删除，天然防重放
    public SsoService(
        IOptions<AppConfig> appConfig,
        ISsoAppManageRepository ssoAppRep,
        ISsoLogRepository ssoLogRep,
        AdminLocalizer adminLocalizer)
    {
        _appConfig = appConfig.Value;
        _ssoAppRep = ssoAppRep;
        _ssoLogRep = ssoLogRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 生成票据（已登录用户点击"单点登录"按钮时调用）
    /// </summary>
    /// <param name="input">第三方应用Id</param>
    /// <returns>票据Ticket</returns>
    [HttpPost]
    public async Task<string> GenerateTicketAsync(SsoTicketInput input)
    {
        if (input == null || input.AppId.IsNull())
            throw ResultOutput.Exception(_adminLocalizer["应用Id不能为空"]);

        var app = await _ssoAppRep.Select.Where(a => a.AppId == input.AppId).ToOneAsync();
        if (app == null)
            throw ResultOutput.Exception(_adminLocalizer["应用不存在"]);
        if (app.Status != 1)
            throw ResultOutput.Exception(_adminLocalizer["应用已禁用"]);

        var ticket = Guid.NewGuid().ToString("N");
        var cacheKey = CacheKeys.GetSsoTicketKey(app.AppId, ticket);

        var ticketInfo = new SsoTicketInfo
        {
            UserId = User.Id,
            UserName = User.UserName,
            Name = User.Name,
            TenantId = User.TenantId,
            AppId = app.AppId,
            CreatedTime = DateTime.Now
        };

        var expireMinutes = _appConfig.Sso.TicketExpireMinutes <= 0 ? 2 : _appConfig.Sso.TicketExpireMinutes;
        await Cache.SetAsync(cacheKey, ticketInfo, TimeSpan.FromMinutes(expireMinutes));

        await WriteLogAsync(ticket, ticketInfo, "success", "生成票据");

        return ticket;
    }

    /// <summary>
    /// 校验并消费票据（第三方系统后端调用，服务端对服务端）
    /// 校验签名、时间戳，消费后返回用户信息
    /// </summary>
    /// <param name="input">AppId + Ticket + Timestamp + Sign</param>
    /// <returns>用户信息</returns>
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<SsoUserInfo> ValidateTicketAsync(SsoValidateInput input)
    {
        if (input == null || input.AppId.IsNull() || input.Ticket.IsNull() || input.Sign.IsNull())
            throw ResultOutput.Exception(_adminLocalizer["参数不完整"]);

        // 时间戳防重放校验（支持 yyyyMMddHHmmsszzz 含时区，兼容 yyyyMMddHHmmss 本地时间）
        var nowTs = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var tsExpire = _appConfig.Sso.TimestampExpireSeconds <= 0 ? 300 : _appConfig.Sso.TimestampExpireSeconds;

        // 优先解析含时区格式（偏移以字符串为准，跨时区无歧义）；兼容仅本地时间格式
        DateTimeOffset inputDt;
        if (!DateTimeOffset.TryParseExact(input.Timestamp, "yyyyMMddHHmmsszzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out inputDt)
            && !DateTimeOffset.TryParseExact(input.Timestamp, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out inputDt))
            throw ResultOutput.Exception(_adminLocalizer["时间戳格式错误"]);

        var inputTs = inputDt.ToUnixTimeSeconds();

        // ✅ 请求过期校验（防重放）：请求时间早于当前超过有效期
        if (nowTs - inputTs > tsExpire)
            throw ResultOutput.Exception(_adminLocalizer["请求已过期"]);

        // ✅ 未来时间校验（防篡改）：允许少量时钟偏差（30秒）
        if (inputTs - nowTs > 30)
            throw ResultOutput.Exception(_adminLocalizer["请求时间异常"]);

        // 2. 查询应用并校验签名（Sign = SM3-HMAC(AppId+Ticket+Timestamp+AppSecret)）
        var app = await _ssoAppRep.Select.Where(a => a.AppId == input.AppId).ToOneAsync();
        if (app == null)
            throw ResultOutput.Exception(_adminLocalizer["应用不存在"]);
        if (app.Status != 1)
            throw ResultOutput.Exception(_adminLocalizer["应用已禁用"]);

        var serverSign = SM3Encryption.ComputeHMACSM3Hex($"{input.AppId}{input.Ticket}{input.Timestamp}{app.AppSecret}", app.AppSecret);
        if (!string.Equals(serverSign, input.Sign, StringComparison.OrdinalIgnoreCase))
            throw ResultOutput.Exception(_adminLocalizer["签名校验失败"]);

        // 3. 获取票据（一次性）
        var cacheKey = CacheKeys.GetSsoTicketKey(input.AppId, input.Ticket);
        var ticketInfo = await Cache.GetAsync<SsoTicketInfo>(cacheKey);
        if (ticketInfo == null)
            throw ResultOutput.Exception(_adminLocalizer["Ticket无效或已过期"]);
        if (ticketInfo.AppId != input.AppId)
            throw ResultOutput.Exception(_adminLocalizer["AppId不匹配"]);

        // 4. 消费（删除）票据，防止重复兑换
        await Cache.DelAsync(cacheKey);

        await WriteLogAsync(input.Ticket, ticketInfo, "success", "消费票据");

        return new SsoUserInfo
        {
            UserId = ticketInfo.UserId,
            UserName = ticketInfo.UserName,
            Name = ticketInfo.Name,
            TenantId = ticketInfo.TenantId,
            AccountId = ticketInfo.AccountId,
            Account = ticketInfo.Account,
        };
    }

    private async Task WriteLogAsync(string ticket, SsoTicketInfo info, string status, string msg)
    {
        try
        {
            var ip = IPHelper.GetIP(AppInfo.HttpContext?.Request);
            await _ssoLogRep.InsertAsync(new SsoLogEntity
            {
                UserId = info.UserId,
                UserName = info.UserName,
                Name = info.Name,
                TenantId = info.TenantId,
                AppId = info.AppId,
                Ticket = ticket,
                Ip = ip,
                Status = status,
                Message = msg
            });
        }
        catch
        {
            // 审计日志写入失败不影响主流程
        }
    }
}
