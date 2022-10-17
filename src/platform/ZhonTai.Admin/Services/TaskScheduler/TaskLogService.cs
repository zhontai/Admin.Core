using System.Threading.Tasks;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Task.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using FreeScheduler;

namespace ZhonTai.Admin.Services.TaskScheduler;

/// <summary>
/// 任务日志服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class TaskLogService : BaseService, ITaskLogService, IDynamicApi
{
    private IRepositoryBase<TaskLog> _taskLogRepository => LazyGetRequiredService<IRepositoryBase<TaskLog>>();

    public TaskLogService()
    {

    }

    /// <summary>
    /// 查询任务日志列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IResultOutput> GetPageAsync(PageInput<TaskLogGetPageDto> input)
    {
        if (!(input.Filter != null && input.Filter.TaskId.NotNull()))
        {
            return ResultOutput.NotOk();
        }

        var list = await _taskLogRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .Where(a => a.TaskId == input.Filter.TaskId)
        .Count(out var total)
        .OrderBy(c => c.Round)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync();

        var data = new PageOutput<TaskLog>()
        {
            List = list,
            Total = total
        };

        return ResultOutput.Ok(data);
    }
}