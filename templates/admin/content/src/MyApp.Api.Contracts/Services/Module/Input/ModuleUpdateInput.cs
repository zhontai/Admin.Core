using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace MyApp.Api.Contracts.Services.Module.Input;

/// <summary>
/// 修改模块
/// </summary>
public partial class ModuleUpdateInput : ModuleAddInput
{
    /// <summary>
    /// 编号
    /// </summary>
    [Required]
    [ValidateRequired("请选择模块")]
    public long Id { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public long Version { get; set; }
}