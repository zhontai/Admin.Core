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
using Yitter.IdGenerator;

namespace ZhonTai.Admin.Services.TaskScheduler;

/// <summary>
/// 任务服务
/// </summary>
[Order(70)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class TaskService : BaseService, ITaskService, IDynamicApi
{
    private readonly Lazy<ITaskRepository> _taskRepository;
    private readonly Lazy<Scheduler> _scheduler;

    public TaskService(Lazy<ITaskRepository> taskRepository, Lazy<Scheduler> scheduler)
    {
        _taskRepository = taskRepository;
        _scheduler = scheduler;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TaskGetOutput> GetAsync(string id)
    {
        var result = await _taskRepository.Value.Where(a => a.Id == id).ToOneAsync<TaskGetOutput>();
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<TaskListOutput>> GetPageAsync(PageInput<TaskGetPageDto> input)
    {
        var topic = input.Filter?.Topic;

        var list = await _taskRepository.Value.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(topic.NotNull(), a => a.Topic.Contains(topic))
        .Count(out var total)
        .OrderByDescending(c => c.Id)
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

        var taskInfo = new TaskInfo
        {
            Id = $"{DateTime.UtcNow.ToString("yyyyMMdd")}.{YitIdHelper.NextId()}",
            Topic = input.Topic,
            Body = input.Body,
            CreateTime = DateTime.UtcNow,
            Round = input.Interval == TaskInterval.Custom ? -1 : input.Round,
            Interval = input.Interval,
            IntervalArgument = input.IntervalArgument,
            CurrentRound = 0,
            ErrorTimes = 0,
            LastRunTime = new DateTime(1970, 1, 1),
            Status = FreeScheduler.TaskStatus.Paused
        };

        await _taskRepository.Value.InsertAsync(taskInfo);

        return taskInfo.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(TaskUpdateInput input)
    {
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
        await _taskRepository.Value.UpdateAsync(entity);

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
    public void Delete([BindRequired][ValidateRequired("请选择任务")] string id)
    {
        var scheduler = _scheduler.Value;
        scheduler.RemoveTask(id);
    }
}