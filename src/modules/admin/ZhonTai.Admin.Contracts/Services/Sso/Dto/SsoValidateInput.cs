namespace ZhonTai.Admin.Contracts.Services.Sso.Dto;

/// <summary>
/// 校验票据输入（第三方系统后端调用）
/// </summary>
public class SsoValidateInput
{
    /// <summary>
    /// 第三方应用Id
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 票据
    /// </summary>
    public string Ticket { get; set; }

    /// <summary>
    /// 请求时间戳（格式：yyyyMMddHHmmsszzz，如 20260713134853+08:00；兼容 yyyyMMddHHmmss 本地时间）
    /// </summary>
    public string Timestamp { get; set; }

    /// <summary>
    /// 签名：SM3-HMAC(AppId + Ticket + Timestamp + AppSecret)，Base64，大小写不敏感
    /// </summary>
    public string Sign { get; set; }
}
