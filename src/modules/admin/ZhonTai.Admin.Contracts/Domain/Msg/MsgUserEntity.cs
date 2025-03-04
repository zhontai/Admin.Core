using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Admin.Domain.Msg;

/// <summary>
/// 消息用户
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "msg_user")]
public partial class MsgUserEntity : EntityBase
{
    /// <summary>
    /// 消息Id
    /// </summary>
    public long MsgId { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    [NotGen]
    [Navigate(nameof(MsgId))]
    public MsgEntity Msg { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    [NotGen]
    [Navigate(nameof(UserId))]
    public MsgUserEntity User { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// 已读时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 是否重要
    /// </summary>
    public bool IsImportant { get; set; }
}