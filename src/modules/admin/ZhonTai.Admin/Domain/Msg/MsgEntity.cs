using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Admin.Domain.Msg;

/// <summary>
/// 消息
/// </summary>
[Table(Name = "base_msg")]
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
    public string TypeId { get; set; }

    /// <summary>
    /// 类型名称
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// 消息状态
    /// </summary>
    public MsgStatusEnum Status { get; set; }
}