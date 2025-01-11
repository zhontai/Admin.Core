using System.Collections.Generic;

namespace ZhonTai.Admin.Core.Configs;

/// <summary>
/// 远程过程调用配置
/// </summary>
public class RpcConfig
{
    public static class Models
    {
        /// <summary>
        /// 地址
        /// </summary>
        public class Endpoint
        {
            /// <summary>
            /// 模块命名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Http地址
            /// </summary>
            public string HttpUrl { get; set; }

            /// <summary>
            /// Grpc地址
            /// </summary>
            public string GrpcUrl { get; set; }
        }
    }

    /// <summary>
    /// 地址列表
    /// </summary>
    public List<Models.Endpoint> Endpoints { get; set; }
}