using ZhonTai.Admin.Domain.Msg;

namespace ZhonTai.Admin.Services.Msg.Dto;

/// <summary>
/// 添加
/// </summary>
public class MsgAddInput
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 类型Id
    /// </summary>
    public long TypeId { get; set; }

    /// <summary>
    /// 类型名称
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// 消息状态
    /// </summary>
    public MsgStatusEnum Status { get; set; }
}