using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Domain.UserStaff;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Domain.UserOrg;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Domain.User;

/// <summary>
/// 用户
/// </summary>
[Table(Name = "ad_user")]
[Index("idx_{tablename}_01", nameof(UserName), true)]
[Index("idx_{tablename}_02", nameof(Mobile))]
[Index("idx_{tablename}_03", nameof(Email))]
public partial class UserEntity : EntityTenant
{
    [NotGen]
    public TenantEntity Tenant { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    [Column(StringLength = 60)]
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Column(StringLength = 200)]
    public string Password { get; set; }

    /// <summary>
    /// 密码加密类型
    /// </summary>
    [Column(MapType = typeof(int?))]
    public PasswordEncryptType? PasswordEncryptType { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Column(StringLength = 60)]
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [Column(StringLength = 20)]
    public string Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Column(StringLength = 100)]
    public string Email { get; set; }

    /// <summary>
    /// 主属部门Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    [NotGen]
    public OrgEntity Org { get; set; }

    /// <summary>
    /// 直属主管Id
    /// </summary>
    public long? ManagerUserId { get; set; }

    /// <summary>
    /// 直属主管
    /// </summary>
    [NotGen]
    public UserEntity ManagerUser { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [Column(StringLength = 60)]
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [Column(StringLength = 500)]
    public string Avatar { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    [Column(MapType = typeof(int?))]
    public UserStatus? Status { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    [Column(MapType = typeof(int))]
    public UserType Type { get; set; } = UserType.DefaultUser;

    /// <summary>
    /// 最后登录时间
    /// </summary>
    [Column(StringLength = 100)]
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    [Column(StringLength = 100)]
    public string LastLoginIP { get; set; }

    /// <summary>
    /// 最后登录国家
    /// </summary>
    [Column(StringLength = 100)]
    public string LastLoginCountry { get; set; }

    /// <summary>
    /// 最后登录省份
    /// </summary>
    [Column(StringLength = 100)]
    public string LastLoginProvince { get; set; }

    /// <summary>
    /// 最后登录城市
    /// </summary>
    [Column(StringLength = 100)]
    public string LastLoginCity { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 角色列表
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(UserRoleEntity))]
    public ICollection<RoleEntity> Roles { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(UserOrgEntity))]
    public ICollection<OrgEntity> Orgs { get; set; }

    /// <summary>
    /// 员工
    /// </summary>
    [NotGen]
    [Navigate(nameof(Id))]
    public UserStaffEntity Staff { get; set; }
}