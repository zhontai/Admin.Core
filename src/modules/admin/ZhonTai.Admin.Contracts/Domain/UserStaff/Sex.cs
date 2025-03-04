using System.ComponentModel;

namespace ZhonTai.Admin.Domain.UserStaff;

/// <summary>
/// 性别
/// </summary>
public enum Sex
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    Unknown = 0,

    /// <summary>
    /// 男
    /// </summary>
    [Description("男")]
    Male = 1,

    /// <summary>
    /// 女
    /// </summary>
    [Description("女")]
    Female = 2
}
