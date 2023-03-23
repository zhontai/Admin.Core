namespace ZhonTai.Admin.Core.Configs;

/// <summary>
/// RabbitMq配置
/// </summary>
public class RabbitMqConfig
{
    /// <summary>
    /// 主机IP或Url地址
    /// </summary>
    public string HostName { get; set; } = string.Empty;

    /// <summary>
    /// 端口号
    /// </summary>
    public int Port { get; set; } = 5672;

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    ///  密码
    /// </summary>
    public string Password { get; set; } = string.Empty;
}