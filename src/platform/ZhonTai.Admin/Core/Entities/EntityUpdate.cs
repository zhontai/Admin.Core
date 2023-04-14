using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 实体修改
/// </summary>
public class EntityUpdate<TKey> : EntityAdd, IEntityUpdate<TKey> where TKey : struct
{
    /// <summary>
    /// 修改者Id
    /// </summary>
    [Description("修改者Id")]
    [Column(Position = -12, CanInsert = false)]
    [JsonProperty(Order = 10000)]
    [JsonPropertyOrder(10000)]
    public virtual long? ModifiedUserId { get; set; }

    /// <summary>
    /// 修改者
    /// </summary>
    [Description("修改者")]
    [Column(Position = -11, CanInsert = false), MaxLength(50)]
    [JsonProperty(Order = 10001)]
    [JsonPropertyOrder(10001)]
    public virtual string ModifiedUserName { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Description("修改时间")]
    [JsonProperty(Order = 10002)]
    [JsonPropertyOrder(10002)]
    [Column(Position = -10, CanInsert = false, ServerTime = DateTimeKind.Local)]
    public virtual DateTime? ModifiedTime { get; set; }
}

/// <summary>
/// 实体修改
/// </summary>
public class EntityUpdate : EntityUpdate<long>
{
}