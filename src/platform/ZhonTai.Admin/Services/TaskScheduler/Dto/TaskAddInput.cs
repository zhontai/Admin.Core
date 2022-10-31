using FreeScheduler;

namespace ZhonTai.Admin.Services.TaskScheduler.Dto;

/// <summary>
/// 添加
/// </summary>
public class TaskAddInput
{
    /// <summary>
    /// 任务标题
    /// </summary>
    public string Topic { get; set; }

    /// <summary>
    /// 任务数据
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// 任务执行多少轮，-1为永久循环
    /// </summary>
    public int Round { get; set; }

    /// <summary>
    /// 定时类型
    /// </summary>
    public TaskInterval Interval { get; set; }

    /// <summary>
    /// 定时参数值 60,60,60,120,120,1200,1200
    /// </summary>
    public string IntervalArgument { get; set; }
}