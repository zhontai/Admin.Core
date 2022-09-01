namespace ZhonTai.Admin.Tools.Cache;

/// <summary>
/// 缓存配置
/// </summary>
public class CacheConfig
{
    /// <summary>
    /// 缓存类型
    /// </summary>
    public CacheType Type { get; set; } = CacheType.Memory;

    /// <summary>
    /// 限流缓存类型
    /// </summary>
    public CacheType TypeRateLimit { get; set; } = CacheType.Memory;

    /// <summary>
    /// Redis配置
    /// </summary>
    public RedisConfig Redis { get; set; } = new RedisConfig();
}

/// <summary>
/// Redis配置
/// </summary>
public class RedisConfig
{
    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = "127.0.0.1:6379,password=,defaultDatabase=2";

    /// <summary>
    /// 限流连接字符串
    /// </summary>
    public string ConnectionStringRateLimit { get; set; } = "127.0.0.1:6379,password=,defaultDatabase=1";
}