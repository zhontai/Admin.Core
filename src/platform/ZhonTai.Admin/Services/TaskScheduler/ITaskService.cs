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
    Task<IResultOutput> GetAsync(long id);

    Task<IResultOutput> GetPageAsync(PageInput<TaskGetPageDto> input);

    IResultOutput Add(TaskAddInput input);

    Task<IResultOutput> UpdateAsync(TaskUpdateInput input);

    IResultOutput Pause(string id);

    IResultOutput Resume(string id);

    IResultOutput Run(string id);

    IResultOutput Delete(string id);
}