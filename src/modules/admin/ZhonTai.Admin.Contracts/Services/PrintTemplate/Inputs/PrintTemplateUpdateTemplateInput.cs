using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.PrintTemplate.Inputs;

/// <summary>
/// 修改模板
/// </summary>
public class PrintTemplateUpdateTemplateInput
{
    /// <summary>
    /// 打印模板Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择打印模板")]
    public long Id { get; set; }

    /// <summary>
    /// 模板
    /// </summary>
    public string Template { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public long Version { get; set; }
}