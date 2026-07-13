namespace ZhonTai.Admin.Contracts.Services.Sso.Dto;

/// <summary>
/// 票据兑换返回的用户信息
/// </summary>
public class SsoUserInfo
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// 浙政钉用户登录账号
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 浙政钉用户账号id
    /// </summary>
    public long AccountId { get; set; }
}
