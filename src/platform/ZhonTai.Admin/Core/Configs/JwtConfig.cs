using System;

namespace ZhonTai.Admin.Core.Configs;

/// <summary>
/// Jwt配置
/// </summary>
public class JwtConfig
{
    /// <summary>
    /// 发行者
    /// </summary>
    public string Issuer { get; set; } = "http://127.0.0.1:8888";

    /// <summary>
    /// 订阅者
    /// </summary>
    public string Audience { get; set; } = "http://127.0.0.1:8888";

    /// <summary>
    /// 密钥
    /// </summary>
    public string SecurityKey { get; set; } = "8efbdb87f52111e08978e9edada709525407095254008978e98efbdb87f52111";

    /// <summary>
    /// 有效期(分钟)
    /// </summary>
    public int Expires { get; set; } = 120;

    /// <summary>
    /// 刷新有效期(分钟)
    /// </summary>
    public int RefreshExpires { get; set; } = 480;
}