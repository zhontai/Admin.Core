namespace ZhonTai.Admin.Services.Tenant.Dto;

/// <summary>
/// 设置启用
/// </summary>
public class TenantSetEnableInput
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public long TenantId { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
}