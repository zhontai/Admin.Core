using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 实体修改
/// </summary>
public class EntityUpdate<TKey> : EntityAdd<TKey>, IEntityUpdate<TKey> where TKey : struct
{
    /// <summary>
    /// 修改者用户Id
    /// </summary>
    [Description("修改者用户Id")]
    [Column(Position = -13)]
    [JsonProperty(Order = 10000)]
    [JsonPropertyOrder(10000)]
    public virtual long? ModifiedUserId { get; set; }

    /// <summary>
    /// 修改者用户名
    /// </summary>
    [Description("修改者用户名")]
    [Column(Position = -12), MaxLength(50)]
    [JsonProperty(Order = 10001)]
    [JsonPropertyOrder(10001)]
    public virtual string ModifiedUserName { get; set; }

    /// <summary>
    /// 修改者姓名
    /// </summary>
    [Description("修改者姓名")]
    [Column(Position = -11), MaxLength(50)]
    [JsonProperty(Order = 10001)]
    [JsonPropertyOrder(10001)]
    public virtual string ModifiedUserRealName { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Description("修改时间")]
    [JsonProperty(Order = 10002)]
    [JsonPropertyOrder(10002)]
    [Column(Position = -10)]
    [ServerTime(CanInsert = false, CanUpdate = true)]
    public virtual DateTime? ModifiedTime { get; set; }
}

/// <summary>
/// 实体修改
/// </summary>
public class EntityUpdate : EntityUpdate<long>
{
}