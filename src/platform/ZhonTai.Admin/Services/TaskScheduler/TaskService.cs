using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.TaskScheduler.Dto;
using ZhonTai.Admin.Domain.Task.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using FreeScheduler;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ZhonTai.Common.Extensions;
using ZhonTai.Admin.Repositories;

namespace ZhonTai.Admin.Services.TaskScheduler;

/// <summary>
/// 任务服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class TaskService : BaseService, ITaskService, IDynamicApi
{
    private ITaskRepository _taskInfoRepository => LazyGetRequiredService<ITaskRepository>();

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
    public IResultOutput Add(TaskAddInput input)
    {
        if (input.IntervalArgument.IsNull())
        {
            return ResultOutput.NotOk("请输入定时参数");
        }

        var scheduler = LazyGetRequiredService<Scheduler>();

        if(input.Interval == TaskInterval.SEC && input.Round == -1)
        {
            scheduler.AddTask(input.Topic, input.Body, input.Round, input.IntervalArgument.ToInt());
        } 
        else if (input.Interval == TaskInterval.SEC && input.Round > 0)
        {
            int[] seconds = System.Array.Empty<int>();
            var intervalArguments = input.IntervalArgument.Split(",");
            foreach(var arg in intervalArguments)
            {
                seconds.Append(arg.ToInt());
            }
            scheduler.AddTask(input.Topic, input.Body, seconds);
        }
        else if (input.Interval == TaskInterval.RunOnDay && input.Round > 0)
        {
            scheduler.AddTaskRunOnDay(input.Topic, input.Body, input.Round, input.IntervalArgument);
        }
        else if (input.Interval == TaskInterval.RunOnWeek && input.Round > 0)
        {
            scheduler.AddTaskRunOnWeek(input.Topic, input.Body, input.Round, input.IntervalArgument);
        }
        else if (input.Interval == TaskInterval.RunOnMonth && input.Round > 0)
        {
            scheduler.AddTaskRunOnMonth(input.Topic, input.Body, input.Round, input.IntervalArgument);
        }
        else if (input.Interval == TaskInterval.Custom && input.Round > 0)
        {
            scheduler.AddTaskCustom(input.Topic, input.Body, input.IntervalArgument);
        }

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
    /// 暂停任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IResultOutput Pause([BindRequired]string id)
    {
        if (id.IsNull())
        {
            return ResultOutput.NotOk();
        }

        var scheduler = LazyGetRequiredService<Scheduler>();
        scheduler.PauseTask(id);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IResultOutput Resume([BindRequired] string id)
    {
        if (id.IsNull())
        {
            return ResultOutput.NotOk();
        }

        var scheduler = LazyGetRequiredService<Scheduler>();
        scheduler.ResumeTask(id);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 执行任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IResultOutput Run([BindRequired] string id)
    {
        if (id.IsNull())
        {
            return ResultOutput.NotOk();
        }

        var scheduler = LazyGetRequiredService<Scheduler>();
        scheduler.RunNowTask(id);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 删除任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IResultOutput Delete([BindRequired] string id)
    {
        if (id.IsNull())
        {
            return ResultOutput.NotOk();
        }

        var scheduler = LazyGetRequiredService<Scheduler>();
        scheduler.RemoveTask(id);

        return ResultOutput.Ok();
    }
}