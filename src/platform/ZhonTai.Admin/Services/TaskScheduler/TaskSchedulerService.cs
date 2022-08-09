using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.TaskScheduler.Dto;
using ZhonTai.Admin.Domain.TaskScheduler.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using FreeScheduler;

namespace ZhonTai.Admin.Services.TaskScheduler
{
    /// <summary>
    /// 任务调度服务
    /// </summary>
    [DynamicApi(Area = AdminConsts.AreaName)]
    public class TaskSchedulerService : BaseService, ITaskSchedulerService, IDynamicApi
    {
        private IRepositoryBase<TaskInfo> _taskInfoRepository => LazyGetRequiredService<IRepositoryBase<TaskInfo>>();
        private IRepositoryBase<TaskLog> _taskLogRepository => LazyGetRequiredService<IRepositoryBase<TaskLog>>();

        public TaskSchedulerService()
        {

        }

        /// <summary>
        /// 查询任务调度
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> GetAsync(long id)
        {
            var result = await _taskInfoRepository.GetAsync<TaskSchedulerGetOutput>(id);
            return ResultOutput.Ok(result);
        }

        /// <summary>
        /// 查询任务调度列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> GetPageAsync(PageInput<TaskSchedulerGetPageDto> input)
        {
            var key = input.Filter?.Name;

            var list = await _taskInfoRepository.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .WhereIf(key.NotNull(), a => a.Topic.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<TaskSchedulerListOutput>();

            var data = new PageOutput<TaskSchedulerListOutput>()
            {
                List = list,
                Total = total
            };

            return ResultOutput.Ok(data);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput> AddAsync(TaskSchedulerAddInput input)
        {
            var entity = Mapper.Map<TaskInfo>(input);
            await _taskInfoRepository.InsertAsync(entity);
            return ResultOutput.Ok();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput> UpdateAsync(TaskSchedulerUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResultOutput.NotOk();
            }

            var entity = await _taskInfoRepository.GetAsync(input.Id);
            if (entity != null && entity.Id.NotNull())
            {
                return ResultOutput.NotOk("任务调度不存在！");
            }

            Mapper.Map(input, entity);
            await _taskInfoRepository.UpdateAsync(entity);
            return ResultOutput.Ok();
        }

        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> DeleteAsync(string id)
        {
            if (id.NotNull())
            {
                await _taskInfoRepository.DeleteAsync(m => m.Id == id);
            }

            return ResultOutput.Ok();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IResultOutput> BatchSoftDeleteAsync(string[] ids)
        {
            if(ids?.Length > 0)
            {
                await _taskInfoRepository.DeleteAsync(a => ids.Contains(a.Id));
            }

            return ResultOutput.Ok();
        }
    }
}