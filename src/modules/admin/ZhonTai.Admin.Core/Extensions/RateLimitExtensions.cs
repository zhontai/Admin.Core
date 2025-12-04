using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Tools.Cache;
using ZhonTai.Common.Helpers;

namespace ZhonTai.Admin.Core.Extensions;

/// <summary>
/// 限流扩展
/// </summary>
public static class RateLimitExtensions
{
    /// <summary>
    /// 添加Ip限流
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="cacheConfig"></param>
    public static void AddIpRateLimit(this IServiceCollection services, IConfiguration configuration, CacheConfig cacheConfig)
    {
        services.Configure<IpRateLimitOptions>(configuration.GetSection("RateLimitConfig:IpRateLimiting"));
        services.Configure<IpRateLimitPolicies>(configuration.GetSection("RateLimitConfig:IpRateLimitPolicies"));

        if (cacheConfig.Type == CacheType.Redis)
        {
            services.AddDistributedRateLimiting();
        }
        else
        {
            services.AddInMemoryRateLimiting();
        }

        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, CacheRateLimitCounterStore>();
    }

    /// <summary>
    /// 添加客户端限流
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="cacheConfig"></param>
    public static void AddClientRateLimit(this IServiceCollection services, IConfiguration configuration, CacheConfig cacheConfig)
    {
        services.Configure<ClientRateLimitOptions>(configuration.GetSection("RateLimitConfig:ClientRateLimiting"));
        services.Configure<ClientRateLimitPolicies>(configuration.GetSection("RateLimitConfig:ClientRateLimitPolicies"));

        if (cacheConfig.Type == CacheType.Redis)
        {
            services.AddDistributedRateLimiting();
        }
        else
        {
            services.AddInMemoryRateLimiting();
        }

        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IRateLimitConfiguration, ClientRateLimitConfiguration>();

        services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, CacheRateLimitCounterStore>();
    }
}

public class ClientResolveContributor: IClientResolveContributor
{
    private readonly RateLimitConfig _rateLimitConfig;

    public ClientResolveContributor(IOptions<RateLimitConfig> rateLimitConfig)
    {
        _rateLimitConfig = rateLimitConfig?.Value;
    }

    public Task<string> ResolveClientAsync(HttpContext httpContext)
    {
        string clientId = null;
        if (httpContext != null && httpContext.Request.Headers.ContainsKey("Authorization"))
        {
            if (_rateLimitConfig.ClientIdType == RateLimitConfig.Enums.ClientIdType.UserId)
            {
                clientId = AppInfo.User.Id.ToString();
            }
            else if (_rateLimitConfig.ClientIdType == RateLimitConfig.Enums.ClientIdType.Token)
            {
                var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                clientId = token;
            }
        }

        return Task.FromResult(clientId);
    }
}

public class ClientRateLimitConfiguration : RateLimitConfiguration
{
    private readonly IOptions<RateLimitConfig> _rateLimitConfig;

    public ClientRateLimitConfiguration(IOptions<IpRateLimitOptions> ipOptions, 
        IOptions<ClientRateLimitOptions> clientOptions,
        IOptions<RateLimitConfig> rateLimitConfig) : base(ipOptions, clientOptions)
    {
        _rateLimitConfig = rateLimitConfig;
    }

    public override void RegisterResolvers()
    {
        base.RegisterResolvers();
        if(_rateLimitConfig.Value.ClientIdType != RateLimitConfig.Enums.ClientIdType.ClientIdHeader)
        {
            ClientResolvers.Add(new ClientResolveContributor(_rateLimitConfig));
        }
    }
}


public class CacheRateLimitCounterStore : IRateLimitCounterStore
{
    private readonly ILogger _logger;
    private readonly ICacheTool _cache;
    private readonly RateLimitConfig _rateLimitConfig;
    private readonly IRateLimitCounterStore _memoryCacheStore;

    public CacheRateLimitCounterStore(
        IMemoryCache memoryCache,
        ILogger<CacheRateLimitCounterStore> logger,
        ICacheTool cache,
        IOptions<RateLimitConfig> rateLimitConfig)
    {
        _logger = logger;
        _cache = cache;
        _rateLimitConfig = rateLimitConfig?.Value;
        _memoryCacheStore = new MemoryCacheRateLimitCounterStore(memoryCache);

    }

    /// <summary>
    /// 获取Redis键
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private string GetRedisKey(string id)
    {
        return $"{_rateLimitConfig.CachePrefix}:{id}";
    }

    public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await TryRedisCommandAsync(
            () =>
            {
                return _cache.ExistsAsync(GetRedisKey(id));
            },
            () =>
            {
                return _memoryCacheStore.ExistsAsync(GetRedisKey(id), cancellationToken);
            });
    }

    public async Task<RateLimitCounter?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await TryRedisCommandAsync(
            async () =>
            {
                var value = await _cache.GetAsync(GetRedisKey(id));

                if (!string.IsNullOrEmpty(value))
                {
                    return JsonHelper.Deserialize<RateLimitCounter?>(value);
                }

                return null;
            },
            () =>
            {
                return _memoryCacheStore.GetAsync(GetRedisKey(id), cancellationToken);
            });
    }

    public async Task RemoveAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = await TryRedisCommandAsync(
            async () =>
            {
                await _cache.DelAsync(GetRedisKey(id));

                return true;
            },
            async () =>
            {
                await _memoryCacheStore.RemoveAsync(GetRedisKey(id), cancellationToken);

                return true;
            });
    }

    public async Task SetAsync(string id, RateLimitCounter? entry, TimeSpan? expirationTime = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = await TryRedisCommandAsync(
            async () =>
            {
                await _cache.SetAsync(GetRedisKey(id), JsonHelper.Serialize(entry.Value), expirationTime);

                return true;
            },
            async () =>
            {
                await _memoryCacheStore.SetAsync(GetRedisKey(id), entry, expirationTime, cancellationToken);

                return true;
            });
    }

    private async Task<T> TryRedisCommandAsync<T>(Func<Task<T>> command, Func<Task<T>> fallbackCommand)
    {
        try
        {
            return await command();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Redis command failed: {ex}");
        }

        return await fallbackCommand();
    }
}