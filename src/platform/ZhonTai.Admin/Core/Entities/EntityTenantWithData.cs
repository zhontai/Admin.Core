using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 实体租户数据权限
/// </summary>
public class EntityTenantWithData<TKey> : EntityTenant, IData
{
    /// <summary>
    /// 拥有者Id
    /// </summary>
    [Description("拥有者Id")]
    [Column(Position = -41)]
    public virtual long? OwnerId { get; set; }

    /// <summary>
    /// 拥有者部门Id
    /// </summary>
    [Description("拥有者部门Id")]
    [Column(Position = -40)]
    public virtual long? OwnerOrgId { get; set; }
}

/// <summary>
/// 实体租户数据权限
/// </summary>
public class EntityTenantWithData : EntityTenantWithData<long>
{
}