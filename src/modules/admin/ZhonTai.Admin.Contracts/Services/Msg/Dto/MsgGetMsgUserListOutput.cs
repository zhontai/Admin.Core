namespace ZhonTai.Admin.Services.Msg.Dto;

/// <summary>
/// 消息用户列表
/// </summary>
public class MsgGetMsgUserListOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// 已读时间
    /// </summary>
    public DateTime? ReadTime { get; set; }
}