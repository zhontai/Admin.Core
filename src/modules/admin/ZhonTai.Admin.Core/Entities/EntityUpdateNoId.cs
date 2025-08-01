using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 实体修改无主键Id
/// </summary>
public class EntityUpdateNoId : EntityAddNoId, IEntityUpdate
{
    /// <summary>
    /// 修改者用户Id
    /// </summary>
    [Description("修改者用户Id")]
    [Column(Position = -13)]
    [JsonPropertyOrder(10000)]
    public virtual long? ModifiedUserId { get; set; }

    /// <summary>
    /// 修改者用户名
    /// </summary>
    [Description("修改者用户名")]
    [Column(Position = -12), MaxLength(60)]
    [JsonPropertyOrder(10001)]
    public virtual string ModifiedUserName { get; set; }

    /// <summary>
    /// 修改者姓名
    /// </summary>
    [Description("修改者姓名")]
    [Column(Position = -11), MaxLength(60)]
    [JsonPropertyOrder(10001)]
    public virtual string ModifiedUserRealName { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Description("修改时间")]
    [JsonPropertyOrder(10002)]
    [Column(Position = -10)]
    [ServerTime(CanInsert = false, CanUpdate = true)]
    public virtual DateTime? ModifiedTime { get; set; }
}