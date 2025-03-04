using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Msg.Dto;

/// <summary>
/// 添加消息用户列表
/// </summary>
public class MsgAddMsgUserListInput
{
    /// <summary>
    /// 消息
    /// </summary>
    [Required(ErrorMessage = "请选择消息")]
    public long MsgId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    public long[] UserIds { get; set; }
}