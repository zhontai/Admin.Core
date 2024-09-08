using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Domain.Role;

namespace ZhonTai.Admin.Domain;

/// <summary>
/// 角色部门
/// </summary>
[Table(Name = "ad_role_org")]
[Index("idx_{tablename}_01", nameof(RoleId) + "," + nameof(OrgId), true)]
public partial class RoleOrgEntity : EntityAdd
{
    /// <summary>
    /// 角色Id
    /// </summary>
	public long RoleId { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    public RoleEntity Role { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
	public long OrgId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public OrgEntity Org { get; set; }
}