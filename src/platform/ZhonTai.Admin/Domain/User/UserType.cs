namespace ZhonTai.Admin.Domain.User;

/// <summary>
/// 用户类型
/// </summary>
public enum UserType
{
    /// <summary>
    /// 默认用户
    /// </summary>
    DefaultUser = 1,

    /// <summary>
    /// 租户管理员
    /// </summary>
    TenantAdmin = 10,

    /// <summary>
    /// 平台管理员
    /// </summary>
    PlatformAdmin = 100
}
