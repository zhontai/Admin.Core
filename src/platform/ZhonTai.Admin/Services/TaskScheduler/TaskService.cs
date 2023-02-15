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
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.TaskScheduler;

/// <summary>
/// 任务服务
/// </summary>
[Order(70)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class TaskService : BaseService, ITaskService, IDynamicApi
{
    private ITaskRepository _taskInfoRepository => LazyGetRequiredService<ITaskRepository>();

    public TaskService()
    {

    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TaskGetOutput> GetAsync(long id)
    {
        var result = await _taskInfoRepository.GetAsync<TaskGetOutput>(id);
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

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string Add(TaskAddInput input)
    {
        if (input.IntervalArgument.IsNull())
        {
            throw ResultOutput.Exception("请输入定时参数");
        }

        var scheduler = LazyGetRequiredService<Scheduler>();

        string id = null;
        switch (input.Interval)
        {
            case TaskInterval.SEC when input.Round == -1:
                id = scheduler.AddTask(input.Topic, input.Body, input.Round, input.IntervalArgument.ToInt());
                break;
            case TaskInterval.SEC when input.Round > 0:
                {
                    int[] seconds = System.Array.Empty<int>();
                    var intervalArguments = input.IntervalArgument.Split(",");
                    foreach (var arg in intervalArguments)
                    {
                        seconds.Append(arg.ToInt());
                    }
                    id = scheduler.AddTask(input.Topic, input.Body, seconds);
                    break;
                }
            case TaskInterval.RunOnDay when input.Round > 0:
                id = scheduler.AddTaskRunOnDay(input.Topic, input.Body, input.Round, input.IntervalArgument);
                break;
            case TaskInterval.RunOnWeek when input.Round > 0:
                id = scheduler.AddTaskRunOnWeek(input.Topic, input.Body, input.Round, input.IntervalArgument);
                break;
            case TaskInterval.RunOnMonth when input.Round > 0:
                id = scheduler.AddTaskRunOnMonth(input.Topic, input.Body, input.Round, input.IntervalArgument);
                break;
            case TaskInterval.Custom when input.Round > 0:
                id = scheduler.AddTaskCustom(input.Topic, input.Body, input.IntervalArgument);
                break;
        }

        return id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(TaskUpdateInput input)
    {
        var entity = await _taskInfoRepository.GetAsync(a => a.Id == input.Id);
        if (entity != null && entity.Id.NotNull())
        {
            throw ResultOutput.Exception("任务不存在！");
        }

        Mapper.Map(input, entity);
        await _taskInfoRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 暂停任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void Pause([BindRequired][ValidateRequired("请选择任务")]string id)
    {
        var scheduler = LazyGetRequiredService<Scheduler>();
        scheduler.PauseTask(id);
    }

    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void Resume([BindRequired][ValidateRequired("请选择任务")] string id)
    {
        var scheduler = LazyGetRequiredService<Scheduler>();
        scheduler.ResumeTask(id);
    }

    /// <summary>
    /// 执行任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void Run([BindRequired][ValidateRequired("请选择任务")] string id)
    {
        var scheduler = LazyGetRequiredService<Scheduler>();
        scheduler.RunNowTask(id);
    }

    /// <summary>
    /// 删除任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void Delete([BindRequired][ValidateRequired("请选择任务")] string id)
    {
        var scheduler = LazyGetRequiredService<Scheduler>();
        scheduler.RemoveTask(id);
    }
}