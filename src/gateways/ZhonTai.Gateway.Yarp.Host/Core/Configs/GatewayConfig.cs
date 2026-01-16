using static ZhonTai.Gateway.Yarp.Host.Core.Configs.GatewayConfig.Models;

namespace ZhonTai.Gateway.Yarp.Host.Core.Configs;

/// <summary>
/// 网关配置
/// </summary>
public class GatewayConfig
{
    public static class Models
    {
        /// <summary>
        /// 模块信息
        /// </summary>
        public class ModuleInfo
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 路径
            /// </summary>
            public string Url { get; set; }
        }

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

        /// <summary>
        /// Kestrel服务器配置
        /// </summary>
        public class KestrelConfig
        {
            /// <summary>
            /// HTTP连接保活最长时间，单位秒
            /// </summary>
            public double KeepAliveTimeout { get; set; } = 130;

            /// <summary>
            /// 发送请求头最长时间，单位秒
            /// </summary>
            public double RequestHeadersTimeout { get; set; } = 30;

            /// <summary>
            /// 最大请求大小，单位bytes
            /// </summary>
            public long? MaxRequestBodySize { get; set; } = 30000000;
        }
    }

    /// <summary>
    /// 模块列表
    /// </summary>
    public List<Models.ModuleInfo> ModuleList { get; set; }

    /// <summary>
    /// 健康检查配置
    /// </summary>
    public Models.HealthChecksConfig HealthChecks { get; set; } = new HealthChecksConfig();

    /// <summary>
    /// Kestrel服务器
    /// </summary>
    public KestrelConfig Kestrel { get; set; } = new KestrelConfig();
}
