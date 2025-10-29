using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProject.Dtos;

/// <summary>项目新增输入</summary>
public partial class DevProjectAddInput
{
    /// <summary>使用模板组</summary>
    [ValidateRequired(ErrorMessage = "使用模板组不能为空")]
    public long GroupId { get; set; }
    /// <summary>项目名称</summary>
    [Required(ErrorMessage = "项目名称不能为空")]
    public string Name { get; set; }
    /// <summary>项目编码</summary>
    [Required(ErrorMessage = "项目编码不能为空")]
    public string Code { get; set; }
    /// <summary>是否启用</summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public bool IsEnable { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }
}