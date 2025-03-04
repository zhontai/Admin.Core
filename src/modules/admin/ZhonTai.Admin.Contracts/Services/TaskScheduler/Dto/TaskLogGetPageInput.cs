namespace ZhonTai.Admin.Domain.Task.Dto;

/// <summary>
/// 任务日志分页请求
/// </summary>
public partial class TaskLogGetPageInput
{
    /// <summary>
    /// 任务Id
    /// </summary>
    public string TaskId { get; set; }
}