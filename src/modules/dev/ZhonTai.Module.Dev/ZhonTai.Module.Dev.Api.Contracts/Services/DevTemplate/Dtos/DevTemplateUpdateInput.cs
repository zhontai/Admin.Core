using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevTemplate.Dtos;

/// <summary>模板更新数据输入</summary>
public partial class DevTemplateUpdateInput
{
    [Required(ErrorMessage = "请选择模板")]
    public long Id { get; set; }
    /// <summary>模板名称</summary>
    [Required(ErrorMessage = "模板名称不能为空")]
    public string Name { get; set; }
    /// <summary>模板分组</summary>
    [Required(ErrorMessage = "模板分组不能为空")]
    public long GroupId { get; set; }
    /// <summary>生成路径</summary>
    public string? OutTo { get; set; }
    /// <summary>是否启用</summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public bool IsEnable { get; set; }
    /// <summary>模板内容</summary>
    [Required(ErrorMessage = "模板内容不能为空")]
    public string Content { get; set; }
}