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
using ZhonTai.Admin.Repositories;
using ZhonTai.Admin.Core.Validators;
using ZhonTai.Admin.Domain;
using Mapster;

namespace ZhonTai.Admin.Services.TaskScheduler;

/// <summary>
/// 任务服务
/// </summary>
[Order(70)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class TaskService : BaseService, ITaskService, IDynamicApi
{
    private readonly Scheduler _scheduler;
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskExtRepository _taskExtRepository;

    public TaskService(Scheduler scheduler, 
        ITaskRepository taskRepository, 
        ITaskExtRepository taskExtRepository
    )
    {
        _scheduler = scheduler;
        _taskRepository = taskRepository;
        _taskExtRepository = taskExtRepository;
    }

    /// <summary>
    /// 查询报警邮件
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<string> GetAlerEmailAsync(string id)
    {
        return await _taskExtRepository.Where(a => a.TaskId == id).ToOneAsync(a => a.AlarmEmail);
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TaskGetOutput> GetAsync(string id)
    {
        var taskInfo = Datafeed.GetTask(_scheduler, id);

        if (taskInfo == null)
        {
            throw ResultOutput.Exception("任务不存在");
        }

        var taskGetOutput = taskInfo.Adapt<TaskGetOutput>();
        taskGetOutput.AlarmEmail = await GetAlerEmailAsync(taskInfo.Id);
        return taskGetOutput;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<TaskListOutput>> GetPage(PageInput<TaskGetPageInput> input)
    {
        //var result = Datafeed.GetPage(_scheduler.Value,
        //    input.Filter.ClusterId,
        //    input.Filter.Topic,
        //    input.Filter.TaskStatus,
        //    input.Filter.StartAddTime,
        //    input.Filter.EndAddTime,
        //    input.PageSize,
        //    input.CurrentPage
        //);

        //var data = new PageOutput<TaskListOutput>()
        //{
        //    List = result.Tasks.Adapt<List<TaskListOutput>>(),
        //    Total = result.Total
        //};

        var taskName = input.Filter?.TaskName;
        var groupName = input.Filter?.GroupName;
        var taskStatus = input.Filter?.TaskStatus;
        var startAddTime = input.Filter?.StartAddTime;
        var endAddTime = input.Filter?.EndAddTime;

        var list = await _taskRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(groupName.NotNull(), a => a.Topic.Contains(groupName))
        .WhereIf(taskName.NotNull(), a => a.Topic.Contains(taskName))
        .WhereIf(taskStatus.HasValue,a=> a.Status == taskStatus)
        .WhereIf(startAddTime.HasValue && !endAddTime.HasValue, a => a.CreateTime >= startAddTime)
        .WhereIf(endAddTime.HasValue && !startAddTime.HasValue, a => a.CreateTime < endAddTime.Value.AddDays(1))
        .WhereIf(startAddTime.HasValue && endAddTime.HasValue, a => a.CreateTime.BetweenEnd(startAddTime.Value, endAddTime.Value.AddDays(1)))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<TaskListOutput>();

        var data = new PageOutput<TaskListOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<string> Add(TaskAddInput input)
    {
        if (input.IntervalArgument.IsNull())
        {
            throw ResultOutput.Exception("请输入定时参数");
        }

        var scheduler = _scheduler;

        var taskld = Datafeed.AddTask(scheduler, input.Topic, input.Body, input.Round, input.Interval, input.IntervalArgument);

        if (taskld.NotNull())
        {
            Pause(taskld);

            await _taskExtRepository.InsertAsync(new TaskInfoExt
            {
                TaskId = taskld,
                AlarmEmail = input.AlarmEmail
            });
        }

        return taskld;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(TaskUpdateInput input)
    {
        var scheduler = _scheduler;

        var entity = await _taskRepository.GetAsync(a => a.Id == input.Id);
        if (entity == null)
        {
            throw ResultOutput.Exception("任务不存在");
        }

        if (entity.Status == FreeScheduler.TaskStatus.Running)
        {
            Pause(entity.Id);
        }

        Mapper.Map(input, entity);

        if (entity.Status == FreeScheduler.TaskStatus.Completed)
        {
            entity.Status = FreeScheduler.TaskStatus.Paused;
        }

        await _taskRepository.UpdateAsync(entity);

        var taskExt = await _taskExtRepository.Select.WhereDynamic(entity.Id).ToOneAsync();
        if(taskExt != null)
        {
            taskExt.AlarmEmail = input.AlarmEmail;
            await _taskExtRepository.UpdateAsync(taskExt);
        }
        else
        {
            await _taskExtRepository.InsertAsync(new TaskInfoExt
            {
                TaskId = entity.Id,
                AlarmEmail = input.AlarmEmail
            });
        }

        if (entity.Status != FreeScheduler.TaskStatus.Paused)
        {
            Resume(entity.Id);
        }
    }

    /// <summary>
    /// 暂停任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void Pause([BindRequired][ValidateRequired("请选择任务")]string id)
    {
        var scheduler = _scheduler;
        scheduler.PauseTask(id);
    }

    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void Resume([BindRequired][ValidateRequired("请选择任务")] string id)
    {
        var scheduler = _scheduler;
        scheduler.ResumeTask(id);
    }

    /// <summary>
    /// 执行任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void Run([BindRequired][ValidateRequired("请选择任务")] string id)
    {
        var scheduler = _scheduler;
        scheduler.RunNowTask(id);
    }

    /// <summary>
    /// 删除任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task Delete([BindRequired][ValidateRequired("请选择任务")] string id)
    {
        var scheduler = _scheduler;
        scheduler.RemoveTask(id);

        await _taskExtRepository.DeleteAsync(a => a.TaskId == id);
    }

    /// <summary>
    /// 批量执行任务
    /// </summary>
    /// <param name="ids"></param>
    public void BatchRun([BindRequired][ValidateRequired("请选择任务")] string[] ids)
    {
        foreach (var id in ids)
        {
            Run(id);
        }
    }

    /// <summary>
    /// 批量暂停任务
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public void BatchPause([BindRequired][ValidateRequired("请选择任务")] string[] ids)
    {
        foreach (var id in ids)
        {
            Pause(id);
        }
    }

    /// <summary>
    /// 批量启动任务
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public void BatchResume([BindRequired][ValidateRequired("请选择任务")] string[] ids)
    {
        foreach (var id in ids)
        {
            Resume(id);
        }
    }

    /// <summary>
    /// 批量删除任务
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchDelete([BindRequired][ValidateRequired("请选择任务")] string[] ids)
    {
        foreach (var id in ids)
        {
            await Delete(id);
        }
    }
}