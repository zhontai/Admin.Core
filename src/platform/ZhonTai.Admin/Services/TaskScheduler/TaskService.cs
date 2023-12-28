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
using System;
using ZhonTai.Admin.Domain;
using Mapster;
using System.Collections.Generic;
using System.Linq;

namespace ZhonTai.Admin.Services.TaskScheduler;

/// <summary>
/// 任务服务
/// </summary>
[Order(70)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class TaskService : BaseService, ITaskService, IDynamicApi
{
    private readonly Lazy<Scheduler> _scheduler;
    private readonly Lazy<ITaskRepository> _taskRepository;
    private readonly Lazy<ITaskExtRepository> _taskExtRepository;

    public TaskService(Lazy<Scheduler> scheduler, Lazy<ITaskRepository> taskRepository, Lazy<ITaskExtRepository> taskExtRepository)
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
        return await _taskExtRepository.Value.Where(a => a.TaskId == id).ToOneAsync(a => a.AlarmEmail);
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="topic"></param>
    /// <returns></returns>
    public async Task<TaskGetOutput> GetAsync(string topic)
    {
        var result = Datafeed.GetPage(_scheduler.Value, null, topic);
        var taskInfo = result.Tasks?.FirstOrDefault()?.Adapt<TaskGetOutput>();
        if (taskInfo != null)
        {
            taskInfo.AlarmEmail = await GetAlerEmailAsync(taskInfo.Id);
        }

        return taskInfo;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public PageOutput<TaskListOutput> GetPage(PageInput<TaskGetPageDto> input)
    {
        var result = Datafeed.GetPage(_scheduler.Value,
            input.Filter.ClusterId,
            input.Filter.Topic,
            input.Filter.TaskStatus,
            input.Filter.StartAddTime,
            input.Filter.EndAddTime,
            input.PageSize,
            input.CurrentPage
        );

        var data = new PageOutput<TaskListOutput>()
        {
            List = result.Tasks.Adapt<List<TaskListOutput>>(),
            Total = result.Total
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

        var scheduler = _scheduler.Value;

        var taskld = Datafeed.AddTask(scheduler, input.Topic, input.Body, input.Round, input.Interval, input.IntervalArgument);

        if (taskld.NotNull())
        {
            Pause(taskld);

            await _taskExtRepository.Value.InsertAsync(new TaskInfoExt
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
        var scheduler = _scheduler.Value;

        var entity = await _taskRepository.Value.GetAsync(a => a.Id == input.Id);
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

        await _taskRepository.Value.UpdateAsync(entity);

        var taskExt = await _taskExtRepository.Value.Select.WhereDynamic(entity.Id).ToOneAsync();
        if(taskExt != null)
        {
            taskExt.AlarmEmail = input.AlarmEmail;
            await _taskExtRepository.Value.UpdateAsync(taskExt);
        }
        else
        {
            await _taskExtRepository.Value.InsertAsync(new TaskInfoExt
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
        var scheduler = _scheduler.Value;
        scheduler.PauseTask(id);
    }

    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void Resume([BindRequired][ValidateRequired("请选择任务")] string id)
    {
        var scheduler = _scheduler.Value;
        scheduler.ResumeTask(id);
    }

    /// <summary>
    /// 执行任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void Run([BindRequired][ValidateRequired("请选择任务")] string id)
    {
        var scheduler = _scheduler.Value;
        scheduler.RunNowTask(id);
    }

    /// <summary>
    /// 删除任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task Delete([BindRequired][ValidateRequired("请选择任务")] string id)
    {
        var scheduler = _scheduler.Value;
        scheduler.RemoveTask(id);

        await _taskExtRepository.Value.DeleteAsync(a => a.TaskId == id);
    }
}