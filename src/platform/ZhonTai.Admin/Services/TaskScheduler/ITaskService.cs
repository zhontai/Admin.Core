using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Task.Dto;
using ZhonTai.Admin.Services.TaskScheduler.Dto;

namespace ZhonTai.Admin.Services.TaskScheduler
{
    /// <summary>
    /// 任务接口
    /// </summary>
    public interface ITaskService
    {
        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetPageAsync(PageInput<TaskGetPageDto> input);

        Task<IResultOutput> AddAsync(TaskAddInput input);

        Task<IResultOutput> UpdateAsync(TaskUpdateInput input);

        Task<IResultOutput> DeleteAsync(string id);

        Task<IResultOutput> BatchDeleteAsync(string[] ids);
    }
}