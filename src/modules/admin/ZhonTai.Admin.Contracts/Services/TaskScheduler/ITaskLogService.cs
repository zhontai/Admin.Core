using FreeScheduler;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Task.Dto;

namespace ZhonTai.Admin.Services.TaskScheduler;

/// <summary>
/// 任务日志接口
/// </summary>
public interface ITaskLogService
{
    PageOutput<TaskLog> GetPage(PageInput<TaskLogGetPageDto> input);

    void Add(TaskLog input);
}