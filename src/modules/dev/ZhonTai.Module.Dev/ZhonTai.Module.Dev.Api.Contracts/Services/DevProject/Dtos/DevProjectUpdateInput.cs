using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProject.Dtos;

/// <summary>项目更新数据输入</summary>
public partial class DevProjectUpdateInput
{
    [Required(ErrorMessage = "请选择项目")]
    public long Id { get; set; }
    /// <summary>项目名称</summary>
    [Required(ErrorMessage = "项目名称不能为空")]
    public string Name { get; set; }
    /// <summary>项目编码</summary>
    [Required(ErrorMessage = "项目编码不能为空")]
    public string Code { get; set; }
    /// <summary>是否启用</summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public bool IsEnable { get; set; }
    /// <summary>使用模板组</summary>
    [Required(ErrorMessage = "使用模板组不能为空")]
    public long GroupId { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }
}