using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevGroup.Dtos;

/// <summary>模板组新增输入</summary>
public partial class DevGroupAddInput
{
    /// <summary>模板组名称</summary>
    [Required(ErrorMessage = "模板组名称不能为空")]
    public string Name { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }
}