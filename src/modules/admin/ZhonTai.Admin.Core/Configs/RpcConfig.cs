namespace ZhonTai.Admin.Core.Configs;

/// <summary>
/// 远程过程调用配置
/// </summary>
public class RpcConfig
{
    public static class Models
    {
        /// <summary>
        /// Http远程配置
        /// </summary>
        public class HttpModel
        {
            /// <summary>
            /// 启用
            /// </summary>
            public bool Enable { get; set; } = true;

            private string[] _assemblyNames;

            /// <summary>
            /// 程序集名称
            /// </summary>
            public string[] AssemblyNames
            {
                get => _assemblyNames;
                set
                {
                    _assemblyNames = value;
                    if (!value.Contains("ZhonTai.Admin.Contracts"))
                    {
                        _assemblyNames = [.. _assemblyNames, "ZhonTai.Admin.Contracts"];
                    }
                }
            }
        }

        /// <summary>
        /// Grpc远程配置
        /// </summary>
        public class GrpcModel
        {
            /// <summary>
            /// 启用
            /// </summary>
            public bool Enable { get; set; } = true;

            private string[] _assemblyNames;

            /// <summary>
            /// 程序集名称
            /// </summary>
            public string[] AssemblyNames
            {
                get => _assemblyNames;
                set
                {
                    _assemblyNames = value;
                    if (!value.Contains("ZhonTai.Admin.Core"))
                    {
                        _assemblyNames = [.. _assemblyNames, "ZhonTai.Admin.Core"];
                    }
                }
            }

            /// <summary>
            /// 服务端程序集名称
            /// </summary>
            public string[] ServerAssemblyNames { get; set; }
        }

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
    /// Http远程配置
    /// </summary>
    public Models.HttpModel Http { get; set; } = new Models.HttpModel();

    /// <summary>
    /// Grpc远程配置
    /// </summary>
    public Models.GrpcModel Grpc { get; set; } = new Models.GrpcModel();

    /// <summary>
    /// 地址列表
    /// </summary>
    public List<Models.Endpoint> Endpoints { get; set; }
}