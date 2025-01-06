namespace ZhonTai.Gateway.Yarp.Core.Configs;

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
    }

    /// <summary>
    /// 模块列表
    /// </summary>
    public List<Models.ModuleInfo> ModuleList { get; set; }
}
