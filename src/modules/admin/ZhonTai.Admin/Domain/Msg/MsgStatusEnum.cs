namespace ZhonTai.Admin.Domain.Msg;

/// <summary>
/// 消息状态
/// </summary>
public enum MsgStatusEnum
{
    /// <summary>
    /// 草稿
    /// </summary>
    Draft = 1,

    /// <summary>
    /// 已发布
    /// </summary>
    Published = 2,

    /// <summary>
    /// 定时发布
    /// </summary>
    Scheduled = 3,

    /// <summary>
    /// 已撤销
    /// </summary>
    Revoked = 4,

    /// <summary>
    /// 已归档
    /// </summary>
    Archived = 5,
}
