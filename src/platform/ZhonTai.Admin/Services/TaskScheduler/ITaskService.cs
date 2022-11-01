using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Task.Dto;
using ZhonTai.Admin.Services.TaskScheduler.Dto;

namespace ZhonTai.Admin.Services.TaskScheduler;

/// <summary>
/// 任务接口
/// </summary>
public interface ITaskService
{
    Task<TaskGetOutput> GetAsync(long id);

    Task<PageOutput<TaskListOutput>> GetPageAsync(PageInput<TaskGetPageDto> input);

    string Add(TaskAddInput input);

    Task UpdateAsync(TaskUpdateInput input);

    void Pause(string id);

    void Resume(string id);

    void Run(string id);

    void Delete(string id);
}