using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevGroup.Dtos;

/// <summary>模板组更新数据输入</summary>
public partial class DevGroupUpdateInput
{
    [Required(ErrorMessage = "请选择模板组")]
    public long Id { get; set; }
    /// <summary>模板组名称</summary>
    [Required(ErrorMessage = "模板组名称不能为空")]
    public string Name { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }
}