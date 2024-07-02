using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Task.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using FreeScheduler;
using Mapster;
using System.Collections.Generic;
using ZhonTai.Admin.Resources;

namespace ZhonTai.Admin.Services.TaskScheduler;

/// <summary>
/// 任务日志服务
/// </summary>
[Order(71)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class TaskLogService : BaseService, ITaskLogService, IDynamicApi
{
    private readonly Scheduler _scheduler;
    private readonly AdminLocalizer _adminLocalizer;

    public TaskLogService(Scheduler scheduler, AdminLocalizer adminLocalizer)
    {
        _scheduler = scheduler;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public PageOutput<TaskLog> GetPage(PageInput<TaskLogGetPageDto> input)
    {
        if (!(input.Filter != null && input.Filter.TaskId.NotNull()))
        {
            throw ResultOutput.Exception(_adminLocalizer["请选择任务"]);
        }

        var result = Datafeed.GetLogs(_scheduler, input.Filter.TaskId, input.PageSize, input.CurrentPage);

        var data = new PageOutput<TaskLog>()
        {
            List = result.Logs.Adapt<List<TaskLog>>(),
            Total = result.Total
        };

        return data;
    }
}