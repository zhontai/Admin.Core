using ZhonTai.Admin.Core.Entities;
using FreeSql;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.Org;
using System.Collections.Generic;
using ZhonTai.Admin.Domain.TenantPkg;
using ZhonTai.Admin.Domain.Pkg;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Domain.Tenant;

/// <summary>
/// 租户
/// </summary>
[Table(Name = "ad_tenant")]
public partial class TenantEntity : EntityBase
{
    /// <summary>
    /// 授权用户
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    [NotGen]
    public UserEntity User { get; set; }

    /// <summary>
    /// 授权部门
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    [NotGen]
    public OrgEntity Org { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    public TenantType? TenantType { get; set; } = Tenant.TenantType.Tenant;

    /// <summary>
    /// 数据库注册键
    /// </summary>
    [Column(StringLength = 50)]
    public string DbKey { get; set; }

    /// <summary>
    /// 数据库
    /// </summary>
    [Column(MapType = typeof(int?))]
    public DataType? DbType { get; set; }

    /// <summary>
    /// 连接字符串
    /// </summary>
    [Column(StringLength = 500)]
    public string ConnectionString { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// 说明
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// 套餐列表
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(TenantPkgEntity))]
    public ICollection<PkgEntity> Pkgs { get; set; }
}