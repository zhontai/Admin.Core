using FreeRedis;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Admin.Tools.Cache;

/// <summary>
/// Redis缓存
/// </summary>
public partial class RedisCacheTool : ICacheTool
{
    [GeneratedRegex("\\{.*\\}")]
    private static partial Regex PatternRegex();

    private readonly RedisClient _redisClient;
    public RedisCacheTool(RedisClient redisClient)
    {
        _redisClient = redisClient;
    }
    public long Del(params string[] key)
    {
        return _redisClient.Del(key);
    }

    public Task<long> DelAsync(params string[] key)
    {
        return _redisClient.DelAsync(key);
    }

    public async Task<long> DelByPatternAsync(string pattern)
    {
        if (pattern.IsNull())
            return default;

        pattern = PatternRegex().Replace(pattern, "*");

        var keys = await _redisClient.KeysAsync(pattern);
        if (keys != null && keys.Length > 0)
        {
            return await _redisClient.DelAsync(keys);
        }

        return default;
    }

    public bool Exists(string key)
    {
        return _redisClient.Exists(key);
    }

    public Task<bool> ExistsAsync(string key)
    {
        return _redisClient.ExistsAsync(key);
    }

    public string Get(string key)
    {
        return _redisClient.Get(key);
    }

    public T Get<T>(string key)
    {
        return _redisClient.Get<T>(key);
    }

    public Task<string> GetAsync(string key)
    {
        return _redisClient.GetAsync(key);
    }

    public Task<T> GetAsync<T>(string key)
    {
        return _redisClient.GetAsync<T>(key);
    }

    public void Set(string key, object value)
    {
        _redisClient.Set(key, value);
    }

    public void Set(string key, object value, TimeSpan expire)
    {
        _redisClient.Set(key, value, expire);
    }

    public Task SetAsync(string key, object value, TimeSpan? expire = null)
    {
        return _redisClient.SetAsync(key, value, expire.HasValue ? expire.Value.TotalSeconds.ToInt() : 0);
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> func, TimeSpan? expire = null)
    {
        if (await _redisClient.ExistsAsync(key))
        {
            try
            {
                return await _redisClient.GetAsync<T>(key);
            }
            catch
            {
                await _redisClient.DelAsync(key);
            }
        }

        var result = await func.Invoke();

        await _redisClient.SetAsync(key, result, expire.HasValue ? expire.Value.TotalSeconds.ToInt() : 0);

        return result;
    }
}