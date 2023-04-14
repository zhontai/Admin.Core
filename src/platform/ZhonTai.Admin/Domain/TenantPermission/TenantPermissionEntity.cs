using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Domain.TenantPermission;

/// <summary>
/// 租户权限
/// </summary>
[Table(Name = "ad_tenant_permission")]
[Index("idx_{tablename}_01", nameof(TenantId) + "," + nameof(PermissionId), true)]
public class TenantPermissionEntity : EntityAdd
{
    /// <summary>
    /// 租户Id
    /// </summary>
	public long TenantId { get; set; }

    /// <summary>
    /// 权限Id
    /// </summary>
	public long PermissionId { get; set; }

    /// <summary>
    /// 租户
    /// </summary>
    [NotGen]
    public TenantEntity Tenant { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    [NotGen]
    public PermissionEntity Permission { get; set; }
}