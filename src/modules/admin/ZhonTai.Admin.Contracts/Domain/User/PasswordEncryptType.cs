namespace ZhonTai.Admin.Domain.User;

/// <summary>
/// 密码加密类型
/// </summary>
public enum PasswordEncryptType
{
    /// <summary>
    /// 32位MD5加密
    /// </summary>
    MD5Encrypt32 = 0,

    /// <summary>
    /// 标准标识密码哈希
    /// </summary>
    PasswordHasher = 1,
}
