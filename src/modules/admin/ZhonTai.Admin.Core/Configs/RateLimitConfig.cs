namespace ZhonTai.Admin.Core.Configs;

/// <summary>
/// 限流配置
/// </summary>
public class RateLimitConfig
{
    public static class Enums
    {
        /// <summary>
        /// 限流方式
        /// </summary>
        public enum RateLimitMethod
        {
            /// <summary>
            /// 无
            /// </summary>
            None = 0,

            /// <summary>
            /// Ip限流
            /// </summary>
            Ip = 1,

            /// <summary>
            /// 客户端限流
            /// </summary>
            Client = 2
        }

        /// <summary>
        /// 客户端Id类型
        /// </summary>
        public enum ClientIdType
        {
            /// <summary>
            /// 无
            /// </summary>
            None = 0,

            /// <summary>
            /// 令牌限流
            /// </summary>
            Token = 1,

            /// <summary>
            /// 用户Id限流
            /// </summary>
            UserId = 2,

            /// <summary>
            /// 请求头限流
            /// </summary>
            ClientIdHeader = 3
        }
    }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// 限流方式
    /// </summary>
    public Enums.RateLimitMethod Method { get; set; } = Enums.RateLimitMethod.Client;

    /// <summary>
    /// 客户端Id类型
    /// </summary>
    public Enums.ClientIdType ClientIdType { get; set; } = Enums.ClientIdType.Token;

    /// <summary>
    /// 缓存前缀
    /// </summary>
    public string CachePrefix { get; set; } = "zhontai:ratelimit";
}
