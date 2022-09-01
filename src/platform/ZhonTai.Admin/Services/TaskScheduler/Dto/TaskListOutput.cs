using FreeScheduler;
using System;

namespace ZhonTai.Admin.Services.TaskScheduler.Dto;

public class TaskListOutput
{
    /// <summary>
    /// 主键
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 任务标题
    /// </summary>
    public string Topic { get; set; }

    /// <summary>
    /// 任务数据
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// 任务执行多少轮
    /// </summary>
    public int Round { get; set; }

    /// <summary>
    /// 定时类型
    /// </summary>
    public TaskInterval Interval { get; set; }

    /// <summary>
    /// 定时参数值
    /// </summary>
    public string IntervalArgument { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaskStatus Status { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 最后运行时间
    /// </summary>
    public DateTime LastRunTime { get; set; }

    /// <summary>
    /// 当前运行到第几轮
    /// </summary>
    public int CurrentRound { get; set; }

    /// <summary>
    /// 错次数
    /// </summary>
    public int ErrorTimes { get; set; }
}