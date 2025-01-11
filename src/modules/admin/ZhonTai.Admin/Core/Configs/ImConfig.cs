namespace ZhonTai.Admin.Core.Configs;

/// <summary>
/// im配置
/// </summary>
public class ImConfig
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// im服务器集群地址
    /// </summary>
    public string[] Servers { get; set; }

    /// <summary>
    /// ws业务端地址
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// Redis连接字符串
    /// </summary>
    public string RedisConnectionString { get; set; }
}
