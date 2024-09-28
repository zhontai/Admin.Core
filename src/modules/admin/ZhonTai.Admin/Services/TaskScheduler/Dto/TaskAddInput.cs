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
    /// 任务参数
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
    /// 定时参数 60,60,60,120,120,1200,1200
    /// </summary>
    public string IntervalArgument { get; set; }

    /// <summary>
    /// 报警邮件，多个邮件地址则逗号分隔
    /// </summary>
    public string AlarmEmail { get; set; }

    /// <summary>
    /// 失败重试次数
    /// </summary>
    public int? FailRetryCount { get; set; }

    /// <summary>
    /// 失败重试间隔（秒）
    /// </summary>
    public int? FailRetryInterval { get; set; }
}