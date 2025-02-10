namespace ZhonTai.Admin.Services.Msg.Dto;

/// <summary>
/// 消息用户列表
/// </summary>
public partial class MsgGetMsgUserListInput
{
   /// <summary>
   /// 消息Id
   /// </summary>
   public long MsgId { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }
}