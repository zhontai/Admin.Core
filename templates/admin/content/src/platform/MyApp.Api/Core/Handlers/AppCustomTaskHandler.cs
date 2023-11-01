using FreeScheduler;
using System;

namespace MyApp.Api.Core.Handlers;

/// <summary>
/// 模块自定义任务处理器
/// </summary>
public class AppCustomTaskHandler : ITaskIntervalCustomHandler
{
    public TimeSpan? NextDelay(TaskInfo task)
    {
        //利用 cron 功能库解析 task.IntervalArgument 得到下一次执行时间
        //与当前时间相减，得到 TimeSpan，若返回 null 则任务完成
        return TimeSpan.FromSeconds(5);
    }
}