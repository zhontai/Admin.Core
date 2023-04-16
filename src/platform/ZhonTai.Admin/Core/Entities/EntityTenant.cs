using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text.Json.Serialization;

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
    [JsonProperty(Order = -20)]
    [JsonPropertyOrder(-20)]
    public virtual long? TenantId { get; set; }
}

/// <summary>
/// 实体租户
/// </summary>
public class EntityTenant : EntityTenant<long>
{
}