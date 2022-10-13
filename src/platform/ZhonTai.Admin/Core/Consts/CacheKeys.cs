using System.ComponentModel;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Core.Consts;

/// <summary>
/// 缓存键
/// </summary>
[ScanCacheKeys]
public static partial class CacheKeys
{
    /// <summary>
    /// 验证码 admin:captcha:guid
    /// </summary>
    [Description("验证码")]
    public const string Captcha = "admin:captcha";

    /// <summary>
    /// 密码加密 admin:password:encrypt:guid
    /// </summary>
    [Description("密码加密")]
    public const string PassWordEncrypt = "admin:password:encrypt";

    /// <summary>
    /// 用户权限 admin:user:permissions:用户主键
    /// </summary>
    [Description("用户权限")]
    public const string UserPermissions = "admin:user:permissions";

    /// <summary>
    /// 用户信息 admin:user:info:用户主键
    /// </summary>
    [Description("用户信息")]
    public const string UserInfo = "admin:user:info";
}