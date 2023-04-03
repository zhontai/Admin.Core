using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Domain.Pkg;

namespace ZhonTai.Admin.Domain.PkgPermission;

/// <summary>
/// 套餐权限
/// </summary>
[Table(Name = "ad_pkg_permission")]
[Index("idx_{tablename}_01", nameof(PkgId) + "," + nameof(PermissionId), true)]
public class PkgPermissionEntity : EntityAdd
{
    /// <summary>
    /// 套餐Id
    /// </summary>
	public long PkgId { get; set; }

    /// <summary>
    /// 套餐
    /// </summary>
    public PkgEntity Pkg { get; set; }

    /// <summary>
    /// 权限Id
    /// </summary>
	public long PermissionId { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    public PermissionEntity Permission { get; set; }
}