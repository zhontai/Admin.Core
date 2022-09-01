using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.TaskScheduler.Dto;
using ZhonTai.Admin.Domain.Task.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using FreeScheduler;

namespace ZhonTai.Admin.Services.TaskScheduler;

/// <summary>
/// 任务服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class TaskService : BaseService, ITaskService, IDynamicApi
{
    private IRepositoryBase<TaskInfo> _taskInfoRepository => LazyGetRequiredService<IRepositoryBase<TaskInfo>>();

    public TaskService()
    {

    }

    /// <summary>
    /// 查询任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetAsync(long id)
    {
        var result = await _taskInfoRepository.GetAsync<TaskGetOutput>(id);
        return ResultOutput.Ok(result);
    }

    /// <summary>
    /// 查询任务列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IResultOutput> GetPageAsync(PageInput<TaskGetPageDto> input)
    {
        var topic = input.Filter?.Topic;

        var list = await _taskInfoRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(topic.NotNull(), a => a.Topic.Contains(topic))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<TaskListOutput>();

        var data = new PageOutput<TaskListOutput>()
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
    public async Task<IResultOutput> AddAsync(TaskAddInput input)
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
    public async Task<IResultOutput> UpdateAsync(TaskUpdateInput input)
    {
        if (input.Id.IsNull())
        {
            return ResultOutput.NotOk();
        }

        var entity = await _taskInfoRepository.GetAsync(a => a.Id == input.Id);
        if (entity != null && entity.Id.NotNull())
        {
            return ResultOutput.NotOk("任务不存在！");
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
        if (id.IsNull())
        {
            return ResultOutput.NotOk();
        }

        await _taskInfoRepository.DeleteAsync(m => m.Id == id);
        
        return ResultOutput.Ok();
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<IResultOutput> BatchDeleteAsync(string[] ids)
    {
        if(!(ids?.Length > 0))
        {
            return ResultOutput.NotOk();
        }

        await _taskInfoRepository.DeleteAsync(a => ids.Contains(a.Id));

        return ResultOutput.Ok();
    }
}