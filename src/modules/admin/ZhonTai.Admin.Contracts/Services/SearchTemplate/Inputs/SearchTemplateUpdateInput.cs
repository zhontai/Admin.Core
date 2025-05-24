using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.SearchTemplate.Inputs;

/// <summary>
/// 修改请求
/// </summary>
public class SearchTemplateUpdateInput
{
    /// <summary>
    /// 查询模板Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择查询模板")]
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 模块Id
    /// </summary>
    public long ModuleId { get; set; }

    /// <summary>
    /// 模板
    /// </summary>
    public string Template { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public long Version { get; set; }
}