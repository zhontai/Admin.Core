using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Task.Dto;

namespace ZhonTai.Admin.Services.TaskScheduler
{
    /// <summary>
    /// 任务日志接口
    /// </summary>
    public interface ITaskLogService
    {
        Task<IResultOutput> GetPageAsync(PageInput<TaskLogGetPageDto> input);
    }
}