using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 实体租户
/// </summary>
public class EntityTenant<TKey> : EntityBase, ITenant
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [Description("租户Id")]
    [Column(Position = 2, CanUpdate = false)]
    public long? TenantId { get; set; }
}

/// <summary>
/// 实体租户
/// </summary>
public class EntityTenant : EntityTenant<long>
{
}