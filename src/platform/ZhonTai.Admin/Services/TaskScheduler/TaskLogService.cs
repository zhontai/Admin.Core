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
using System;
using ZhonTai.Admin.Repositories;

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
    private readonly Lazy<ITaskLogRepository> _taskLogRep;

    public TaskLogService(Scheduler scheduler, 
        AdminLocalizer adminLocalizer,
        Lazy<ITaskLogRepository> taskLogRep)
    {
        _scheduler = scheduler;
        _adminLocalizer = adminLocalizer;
        _taskLogRep = taskLogRep;
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

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [NonAction]
    public void Add(TaskLog input)
    {
        _taskLogRep.Value.InsertAsync(input);
    }
}