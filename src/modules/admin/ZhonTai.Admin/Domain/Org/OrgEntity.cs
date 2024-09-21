using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using System.Collections.Generic;
using ZhonTai.Admin.Domain.UserStaff;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.UserOrg;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Domain.Org;

/// <summary>
/// 组织架构
/// </summary>
[Table(Name = "ad_org")]
[Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Name) + "," + nameof(TenantId), true)]
public partial class OrgEntity : EntityTenant, IChilds<OrgEntity>
{
    /// <summary>
    /// 父级
    /// </summary>
	public long ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [Column(StringLength = 50)]
    public string Code { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    [Column(StringLength = 50)]
    public string Value { get; set; }

    /// <summary>
    /// 成员数
    /// </summary>
    public int MemberCount { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
	public int Sort { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// 员工列表
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(UserOrgEntity))]
    public ICollection<UserStaffEntity> Staffs { get; set; }

    /// <summary>
    /// 用户列表
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(UserOrgEntity))]
    public ICollection<UserEntity> Users { get; set; }

    /// <summary>
    /// 角色列表
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(RoleOrgEntity))]
    public ICollection<RoleEntity> Roles { get; set; }

    /// <summary>
    /// 子级列表
    /// </summary>
    [Navigate(nameof(ParentId))]
    public List<OrgEntity> Childs { get; set; }
}