using FreeScheduler;
using System;

namespace ZhonTai.Admin.Domain.Task.Dto;

public partial class TaskGetPageInput
{
    /// <summary>
    /// 分组名称
    /// </summary>
    public string GroupName { get; set; }

    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; set; }

    /// <summary>
    /// 集群Id
    /// </summary>
    public string ClusterId { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaskStatus? TaskStatus { get; set; }

    /// <summary>
    /// 创建开始时间
    /// </summary>
    public DateTime? StartAddTime { get; set; }

    /// <summary>
    /// 创建结束时间
    /// </summary>
    public DateTime? EndAddTime { get; set; }
}
