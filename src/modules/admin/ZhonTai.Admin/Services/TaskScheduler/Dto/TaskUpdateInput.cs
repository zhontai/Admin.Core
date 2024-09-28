using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.TaskScheduler.Dto;

/// <summary>
/// 修改
/// </summary>
public partial class TaskUpdateInput : TaskAddInput
{
    /// <summary>
    /// 任务Id
    /// </summary>
    [Required(ErrorMessage = "请选择任务")]
    public string Id { get; set; }
}