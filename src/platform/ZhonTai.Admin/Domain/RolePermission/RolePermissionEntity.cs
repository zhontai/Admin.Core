using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.Permission;

namespace ZhonTai.Admin.Domain.RolePermission;

/// <summary>
/// 角色权限
/// </summary>
[Table(Name = "ad_role_permission")]
[Index("idx_{tablename}_01", nameof(RoleId) + "," + nameof(PermissionId), true)]
public class RolePermissionEntity : EntityAdd
{
    /// <summary>
    /// 角色Id
    /// </summary>
	public long RoleId { get; set; }

    /// <summary>
    /// 权限Id
    /// </summary>
	public long PermissionId { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    public RoleEntity Role { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    public PermissionEntity Permission { get; set; }
}