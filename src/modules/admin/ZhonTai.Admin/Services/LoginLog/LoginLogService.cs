using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.LoginLog;
using ZhonTai.Admin.Services.LoginLog.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;

namespace ZhonTai.Admin.Services.LoginLog;

/// <summary>
/// 登录日志服务
/// </summary>
[Order(190)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class LoginLogService : BaseService, ILoginLogService, IDynamicApi, ICapSubscribe
{
    private readonly ILoginLogRepository _loginLogRep;

    public LoginLogService(ILoginLogRepository loginLogRep)
    {
        _loginLogRep = loginLogRep;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<LoginLogGetPageOutput>> GetPageAsync(PageInput<LoginLogGetPageInput> input)
    {
        var select = _loginLogRep.Select.WhereDynamicFilter(input.DynamicFilter);
        if (User.PlatformAdmin)
        {
            select = select.DisableGlobalFilter();
        }

        if(input.Filter != null)
        {
            var addStartTime = input.Filter.AddStartTime;
            var addEndTime = input.Filter.AddEndTime;
            select = select
            .WhereIf(input.Filter.CreatedUserName.NotNull(), a => a.CreatedUserName.Contains(input.Filter.CreatedUserName))
            .WhereIf(input.Filter.Status.HasValue, a => a.Status == input.Filter.Status)
            .WhereIf(input.Filter.IP.NotNull(), a => a.IP.Contains(input.Filter.IP))
            .WhereIf(addStartTime.HasValue && !addEndTime.HasValue, a => a.CreatedTime >= addStartTime)
            .WhereIf(addEndTime.HasValue && !addStartTime.HasValue, a => a.CreatedTime < addEndTime.Value.AddDays(1))
            .WhereIf(addStartTime.HasValue && addEndTime.HasValue, a => a.CreatedTime.Value.BetweenEnd(addStartTime.Value, addEndTime.Value.AddDays(1)))
            ;
        }

        var list = await select
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<LoginLogGetPageOutput>();

        var data = new PageOutput<LoginLogGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [NonAction]
    [CapSubscribe(SubscribeNames.LoginLogAdd)]
    public async Task<long> AddAsync(LoginLogAddInput input)
    {
        var entity = Mapper.Map<LoginLogEntity>(input);
        await _loginLogRep.InsertAsync(entity);

        return entity.Id;
    }
}