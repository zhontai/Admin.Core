using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.Document.Dto;

/// <summary>
/// 更新分组
/// </summary>
public class DocumentUpdateGroupInput : DocumentAddGroupInput
{
    /// <summary>
    /// 编号
    /// </summary>
    [Required]
    [ValidateRequired("请选择分组")]
    public long Id { get; set; }
}