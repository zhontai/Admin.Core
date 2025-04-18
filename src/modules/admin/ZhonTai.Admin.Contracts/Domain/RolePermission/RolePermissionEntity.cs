using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Domain.RolePermission;

/// <summary>
/// 角色权限
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "role_permission", OldName = DbConsts.TableOldNamePrefix + "role_permission")]
[Index("idx_{tablename}_01", nameof(RoleId) + "," + nameof(PermissionId), true)]
public class RolePermissionEntity : EntityAdd
{
    /// <summary>
    /// 平台
    /// </summary>
    public string Platform { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
	[Column(IsPrimary = true)]
    public long RoleId { get; set; }

    /// <summary>
    /// 权限Id
    /// </summary>
	public long PermissionId { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    [NotGen]
    [Column(IsPrimary = true)]
    public RoleEntity Role { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    [NotGen]
    public PermissionEntity Permission { get; set; }
}