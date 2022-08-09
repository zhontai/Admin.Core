using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.TaskScheduler.Dto;
using ZhonTai.Admin.Services.TaskScheduler.Dto;

namespace ZhonTai.Admin.Services.TaskScheduler
{
    /// <summary>
    /// 任务调度接口
    /// </summary>
    public interface ITaskSchedulerService
    {
        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetPageAsync(PageInput<TaskSchedulerGetPageDto> input);

        Task<IResultOutput> AddAsync(TaskSchedulerAddInput input);

        Task<IResultOutput> UpdateAsync(TaskSchedulerUpdateInput input);

        Task<IResultOutput> DeleteAsync(string id);

        Task<IResultOutput> BatchSoftDeleteAsync(string[] ids);
    }
}