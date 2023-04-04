using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Pkg.Dto;

/// <summary>
/// 添加套餐租户列表
/// </summary>
public class PkgAddPkgTenantListInput
{
    /// <summary>
    /// 套餐
    /// </summary>
    [Required(ErrorMessage = "请选择套餐")]
    public long PkgId { get; set; }

    /// <summary>
    /// 租户列表
    /// </summary>
    public long[] TenantIds { get; set; }
}