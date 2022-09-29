using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Domain.Staff;

namespace ZhonTai.Admin.Domain.User;

/// <summary>
/// 用户
/// </summary>
[Table(Name = "ad_user")]
[Index("idx_{tablename}_01", nameof(UserName) + "," + nameof(TenantId), true)]
public partial class UserEntity : EntityFull, ITenant
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [Column(Position = 2)]
    public long? TenantId { get; set; }

    public TenantEntity Tenant { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    [Column(StringLength = 60)]
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Column(StringLength = 60)]
    public string Password { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Column(StringLength = 20)]
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
    /// 昵称
    /// </summary>
    [Column(StringLength = 20)]
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [Column(StringLength = 100)]
    public string Avatar { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    [Column(MapType = typeof(int))]
    public UserStatusEnum Status { get; set; }

    /// <summary>
    /// 角色列表
    /// </summary>
    [Navigate(ManyToMany = typeof(UserRoleEntity))]
    public ICollection<RoleEntity> Roles { get; set; }

    /// <summary>
    /// 员工
    /// </summary>
    [Navigate(nameof(Id))]
    public StaffEntity Emp { get; set; }
}