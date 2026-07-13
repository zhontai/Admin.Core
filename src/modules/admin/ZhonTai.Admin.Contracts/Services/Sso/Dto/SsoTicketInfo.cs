namespace ZhonTai.Admin.Contracts.Services.Sso.Dto;

/// <summary>
/// 票据缓存信息（存入 ICacheTool，不在URL中透传）
/// </summary>
public class SsoTicketInfo
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
    /// 浙政钉用户登录账号
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 浙政钉用户账号id
    /// </summary>
    public long AccountId { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
}
