namespace ZhonTai.Admin.Services.TaskScheduler.Dto;

/// <summary>
/// 修改
/// </summary>
public partial class TaskUpdateInput : TaskAddInput
{
    /// <summary>
    /// 任务Id
    /// </summary>
    public string Id { get; set; }
}