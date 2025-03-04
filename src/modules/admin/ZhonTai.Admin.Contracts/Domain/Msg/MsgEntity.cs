using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Admin.Domain.MsgType;

namespace ZhonTai.Admin.Domain.Msg;

/// <summary>
/// 消息
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "msg")]
public partial class MsgEntity : EntityBase
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [Column(StringLength = -1)]
    public string Content { get; set; }

    /// <summary>
    /// 类型Id
    /// </summary>
    public long TypeId { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [NotGen]
    [Navigate(nameof(TypeId))]
    public MsgTypeEntity Type { get; set; }

    /// <summary>
    /// 类型名称
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// 消息状态
    /// </summary>
    public MsgStatusEnum Status { get; set; }
}