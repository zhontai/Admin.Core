using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.Pkg;

namespace ZhonTai.Admin.Domain.TenantPkg;

/// <summary>
/// 租户套餐
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "tenant_pkg", OldName = DbConsts.TableOldNamePrefix + "tenant_pkg")]
[Index("idx_{tablename}_01", nameof(TenantId) + "," + nameof(PkgId), true)]
public class TenantPkgEntity : EntityAdd
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [Column(IsPrimary = true)]
    public long TenantId { get; set; }

    public TenantEntity Tenant { get; set; }

    /// <summary>
    /// 套餐Id
    /// </summary>
    [Column(IsPrimary = true)]
    public long PkgId { get; set; }

    public PkgEntity Pkg { get; set; }
}