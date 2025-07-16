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
    }

    /// <summary>
    /// 模块列表
    /// </summary>
    public List<Models.ModuleInfo> ModuleList { get; set; }

    /// <summary>
    /// 健康检查配置
    /// </summary>
    public Models.HealthChecksConfig HealthChecks { get; set; } = new Models.HealthChecksConfig();
}
