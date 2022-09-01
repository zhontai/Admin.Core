namespace ZhonTai.Admin.Core.Auth;

/// <summary>
/// Claim属性
/// </summary>
public static class ClaimAttributes
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public const string UserId = "sub";

    /// <summary>
    /// 用户名
    /// </summary>
    public const string UserName = "na";

    /// <summary>
    /// 姓名
    /// </summary>
    public const string UserNickName = "nn";

    /// <summary>
    /// 刷新有效期
    /// </summary>
    public const string RefreshExpires = "re";

    /// <summary>
    /// 租户Id
    /// </summary>
    public const string TenantId = "ti";

    /// <summary>
    /// 租户类型
    /// </summary>
    public const string TenantType = "tt";

    /// <summary>
    /// 数据隔离
    /// </summary>
    public const string DataIsolationType = "dit";
}