using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectModel.Dtos;

/// <summary>项目模型更新数据输入</summary>
public partial class DevProjectModelUpdateInput
{
    [Required(ErrorMessage = "请选择项目模型")]
    public long Id { get; set; }
    /// <summary>所属项目</summary>
    [Required(ErrorMessage = "所属项目不能为空")]
    public long ProjectId { get; set; }
    /// <summary>模型名称</summary>
    [Required(ErrorMessage = "模型名称不能为空")]
    public string Name { get; set; }
    /// <summary>模型编码</summary>
    [Required(ErrorMessage = "模型编码不能为空")]
    public string Code { get; set; }
    /// <summary>是否启用</summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public bool IsEnable { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }
}