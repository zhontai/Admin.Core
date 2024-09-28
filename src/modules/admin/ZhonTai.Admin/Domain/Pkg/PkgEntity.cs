using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.TenantPkg;
using ZhonTai.Admin.Domain.PkgPermission;

namespace ZhonTai.Admin.Domain.Pkg;

/// <summary>
/// 套餐
/// </summary>
[Table(Name = "ad_pkg")]
[Index("idx_{tablename}_01", $"{nameof(ParentId)},{nameof(Name)}", true)]
public partial class PkgEntity : EntityBase
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 子级列表
    /// </summary>
    [Navigate(nameof(ParentId))]
    public List<PkgEntity> Childs { get; set; }

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
    /// 说明
    /// </summary>
    [Column(StringLength = 200)]
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
	public int Sort { get; set; }

    /// <summary>
    /// 租户列表
    /// </summary>
    [Navigate(ManyToMany = typeof(TenantPkgEntity))]
    public ICollection<TenantEntity> Tenants { get; set; }

    /// <summary>
    /// 权限列表
    /// </summary>
    [Navigate(ManyToMany = typeof(PkgPermissionEntity))]
    public ICollection<PermissionEntity> Permissions { get; set; }
}