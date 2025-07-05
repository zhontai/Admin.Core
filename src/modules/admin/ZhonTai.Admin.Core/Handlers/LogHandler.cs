using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using ZhonTai.Admin.Core.Exceptions;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Helpers;
using DotNetCore.CAP;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Services.Api.Dto;
using Microsoft.AspNetCore.Http;
using IP2Region.Net.Abstractions;
using LocationInfo = ZhonTai.Admin.Core.Records.LocationInfo;
using Microsoft.Extensions.Options;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Auth;

namespace ZhonTai.Admin.Core.Handlers;

/// <summary>
/// 操作日志处理
/// </summary>
public class LogHandler : ILogHandler
{
    private readonly ICapPublisher _capPublisher;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger _logger;
    private readonly IOptions<AppConfig> _appConfig;
    private readonly IUser _user;
    private readonly ApiHelper _apiHelper;

    public LogHandler(
        ICapPublisher capPublisher,
        IHttpContextAccessor httpContextAccessor,
        ILogger<LogHandler> logger,
        IOptions<AppConfig> appConfig,
        IUser user,
        ApiHelper apiHelper
    )
    {
        _capPublisher = capPublisher;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _appConfig = appConfig;
        _user = user;
        _apiHelper = apiHelper;
    }

    /// <summary>
    /// 获得IP地址
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    private LocationInfo GetIpLocationInfo(string ip)
    {
        var locationInfo = new LocationInfo();
        if (_appConfig.Value.IP2Region.Enable)
        {
            if (IPHelper.IsIP(ip))
            {
                var region = AppInfo.GetRequiredService<ISearcher>().Search(ip);
                locationInfo = LocationInfo.Parse(region);
            }
        }

        return locationInfo;
    }

    public async Task LogAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        var actionExecutedContext = await next();
        stopwatch.Stop();

        try
        {
            var input = new OperationLogAddInput
            {
                Status = true,
                ApiMethod = context.HttpContext.Request.Method.ToLower(),
                ApiPath = context.ActionDescriptor.AttributeRouteInfo.Template.ToLower(),
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                StatusCode = context.HttpContext.Response.StatusCode
            };

            var api = (await _apiHelper.GetApiListAsync()).FirstOrDefault(a => a.Path == input.ApiPath);
            //操作日志启用
            if ((api != null && api.EnabledLog))
            {
                var excepton = actionExecutedContext.Exception;

                //操作参数
                if ((api != null && api.EnabledParams && context.ActionArguments.Count > 0) || excepton != null)
                {
                    input.Params = JsonHelper.Serialize(context.ActionArguments);
                }

                //操作结果
                if (api != null && api.EnabledResult && actionExecutedContext.Result != null && actionExecutedContext.Result is JsonResult result)
                {
                    input.Result = JsonHelper.Serialize(result.Value);
                }

                if (excepton != null)
                {
                    input.Status = false;

                    var code = "";
                    if (excepton is AppException appException)
                    {
                        input.StatusCode = appException.StatusCode;
                        code = appException.AppCode;
                    }
                    else
                    {
                        input.StatusCode = (int)HttpStatusCode.InternalServerError;
                    }

                    input.Result = JsonHelper.Serialize(new ResultOutput<string>()
                    {
                        Code = code
                    }.NotOk(excepton.Message));
                }

                input.ApiLabel = api?.Label;

                if (input.IP.IsNull())
                {
                    input.IP = IPHelper.GetIP(_httpContextAccessor?.HttpContext?.Request);
                    var locationInfo = GetIpLocationInfo(input.IP);
                    input.Country = locationInfo?.Country;
                    input.Province = locationInfo?.Province;
                    input.City = locationInfo?.City;
                    input.Isp = locationInfo?.Isp;
                }

                string ua = _httpContextAccessor?.HttpContext?.Request?.Headers?.UserAgent;
                if (ua.NotNull())
                {
                    var client = UAParser.Parser.GetDefault().Parse(ua);
                    var device = client.Device.Family;
                    device = device.ToLower() == "other" ? "" : device;
                    input.Browser = client.UA.Family;
                    input.Os = client.OS.Family;
                    input.Device = device;
                    input.BrowserInfo = ua;
                }

                input.Name = _user.Name;
                input.CreatedUserId = _user.Id;
                input.CreatedUserName = _user.UserName;
                input.CreatedUserRealName = _user.Name;
                input.TenantId = _user.TenantId;

                await _capPublisher.PublishAsync(SubscribeNames.OperationLogAdd, input);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("操作日志插入异常：{@ex}", ex);
        }
    }
}