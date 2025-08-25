namespace MyIMServer.Host.Core.Configs;

/// <summary>
/// im服务端配置
/// </summary>
public class ImServerConfig
{
    public static class Models
    {
        /// <summary>
        /// 健康检查配置
        /// </summary>
        public class HealthChecksConfig
        {
            /// <summary>
            /// 启用
            /// </summary>
            public bool Enable { get; set; } = true;

            /// <summary>
            /// 访问路径
            /// </summary>
            public string Path { get; set; } = "/health";
        }
    }

    /// <summary>
    /// Redis客户端连接字符串
    /// </summary>
    public string RedisClientConnectionString { get; set; }

    /// <summary>
    /// 服务端列表
    /// </summary>
    public string[] Servers { get; set; }

    /// <summary>
    /// 服务端
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// 输入编码名称，默认GB2312
    /// </summary>
    public string InputEncodingName { get; set; } = "GB2312";

    /// <summary>
    /// 输出编码名称，默认GB2312
    /// </summary>
    public string OutputEncodingName { get; set; } = "GB2312";

    /// <summary>
    /// 健康检查配置
    /// </summary>
    public Models.HealthChecksConfig HealthChecks { get; set; } = new Models.HealthChecksConfig();
}
