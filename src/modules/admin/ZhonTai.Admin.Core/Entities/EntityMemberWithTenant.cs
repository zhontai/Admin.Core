using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 实体会员租户
/// </summary>
public class EntityMemberWithTenant<TKey> : EntityMember<TKey>, ITenant where TKey : struct
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [Description("租户Id")]
    [Column(Position = 2, CanUpdate = false)]
    [JsonPropertyOrder(-20)]
    public virtual long? TenantId { get; set; }
}

/// <summary>
/// 实体会员租户
/// </summary>
public class EntityMemberWithTenant : EntityMemberWithTenant<long>
{
}