namespace ZhonTai.Admin.Services.Pkg.Dto;

/// <summary>
/// 套餐租户列表请求
/// </summary>
public partial class PkgGetPkgTenantListInput
{
    /// <summary>
    /// 租户名
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// 套餐Id
    /// </summary>
    public long? PkgId { get; set; }
}