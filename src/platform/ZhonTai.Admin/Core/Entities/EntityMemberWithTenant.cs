using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 实体会员租户
/// </summary>
public class EntityMemberWithTenant<TKey> : EntityMember, ITenant
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [Description("租户Id")]
    [Column(Position = 2, CanUpdate = false)]
    [JsonProperty(Order = -20)]
    public virtual long? TenantId { get; set; }
}

/// <summary>
/// 实体会员租户
/// </summary>
public class EntityMemberWithTenant : EntityMemberWithTenant<long>
{
}