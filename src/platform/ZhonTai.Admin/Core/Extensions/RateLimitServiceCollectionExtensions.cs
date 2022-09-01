using AspNetCoreRateLimit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZhonTai.Admin.Tools.Cache;

namespace ZhonTai.Admin.Core.Extensions;

public static class RateLimitServiceCollectionExtensions
{
    /// <summary>
    /// 添加Ip限流
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="cacheConfig"></param>
    public static void AddIpRateLimit(this IServiceCollection services, IConfiguration configuration, CacheConfig cacheConfig)
    {
        #region IP限流

        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

        if (cacheConfig.TypeRateLimit == CacheType.Redis)
        {
            //redis
            var redisRateLimit = new CSRedis.CSRedisClient(cacheConfig.Redis.ConnectionStringRateLimit);
            services.AddSingleton<IDistributedCache>(new CSRedisCache(redisRateLimit));
            services.AddDistributedRateLimiting();
        }
        else
        {
            //内存
            services.AddMemoryCache();
            services.AddInMemoryRateLimiting();
        }
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        #endregion IP限流
    }
}
