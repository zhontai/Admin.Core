namespace ZhonTai.Admin.Domain.User;

/// <summary>
/// 用户状态
/// </summary>
public enum UserStatusEnum
{
    /// <summary>
    /// 正常
    /// </summary>
    Enabled = 0,
    /// <summary>
    /// 禁用
    /// </summary>
    Disabled = 1,
    /// <summary>
    /// 锁定
    /// </summary>
    Locked = 2,
    /// <summary>
    /// 待修改密码
    /// </summary>
    WaitChangePasssword = 3,
    /// <summary>
    /// 待激活
    /// </summary>
    WaitActive = 4,
}
