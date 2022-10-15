using ZhonTai.Admin.Core.Entities;
using FreeSql;
using FreeSql.DataAnnotations;
using System;
using ZhonTai.Admin.Domain.User;

namespace ZhonTai.Admin.Domain.Tenant;

/// <summary>
/// 租户
/// </summary>
[Table(Name = "ad_tenant")]
[Index("idx_{tablename}_01", nameof(Name), true)]
[Index("idx_{tablename}_02", nameof(Code), true)]
public partial class TenantEntity : EntityBase
{
    /// <summary>
    /// 企业名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [Column(StringLength = 50)]
    public string Code { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string RealName { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [Column(StringLength = 20)]
    public string Phone { get; set; }

    /// <summary>
    /// 邮箱地址
    /// </summary>
    [Column(StringLength = 50)]
    public string Email { get; set; }

    /// <summary>
    /// 授权用户
    /// </summary>
    public long? UserId { get; set; }

    public UserEntity User { get; set; }

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
}