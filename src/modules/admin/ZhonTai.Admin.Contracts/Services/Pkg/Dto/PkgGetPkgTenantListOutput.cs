namespace ZhonTai.Admin.Services.Pkg.Dto;

/// <summary>
/// 套餐租户列表响应
/// </summary>
public class PkgGetPkgTenantListOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 租户名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 租户编码
    /// </summary>
    public string Code { get; set; }
}