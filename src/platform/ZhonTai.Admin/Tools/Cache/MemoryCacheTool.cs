using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Admin.Tools.Cache;

/// <summary>
/// 内存缓存
/// </summary>
public class MemoryCacheTool : ICacheTool
{
    private readonly IMemoryCache _memoryCache;
    public MemoryCacheTool(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public long Del(params string[] key)
    {
        foreach (var k in key)
        {
            _memoryCache.Remove(k);
        }
        return key.Length;
    }

    public Task<long> DelAsync(params string[] key)
    {
        foreach (var k in key)
        {
            _memoryCache.Remove(k);
        }

        return Task.FromResult(key.Length.ToLong());
    }

    public async Task<long> DelByPatternAsync(string pattern)
    {
        if (pattern.IsNull())
            return default;

        pattern = Regex.Replace(pattern, @"\{.*\}", "(.*)");

        var keys = GetAllKeys().Where(k => Regex.IsMatch(k, pattern));

        if (keys != null && keys.Count() > 0)
        {
            return await DelAsync(keys.ToArray());
        }

        return default;
    }

    public bool Exists(string key)
    {
        return _memoryCache.TryGetValue(key, out _);
    }

    public Task<bool> ExistsAsync(string key)
    {
        return Task.FromResult(_memoryCache.TryGetValue(key, out _));
    }

    public string Get(string key)
    {
        return _memoryCache.Get(key)?.ToString();
    }

    public T Get<T>(string key)
    {
        return _memoryCache.Get<T>(key);
    }

    public Task<string> GetAsync(string key)
    {
        return Task.FromResult(Get(key));
    }

    public Task<T> GetAsync<T>(string key)
    {
        return Task.FromResult(Get<T>(key));
    }

    public bool Set(string key, object value)
    {
        _memoryCache.Set(key, value);
        return true;
    }

    public bool Set(string key, object value, TimeSpan expire)
    {
        _memoryCache.Set(key, value, expire);
        return true;
    }

    public Task<bool> SetAsync(string key, object value)
    {
        Set(key, value);
        return Task.FromResult(true);
    }

    public Task<bool> SetAsync(string key, object value, TimeSpan expire)
    {
        Set(key, value, expire);
        return Task.FromResult(true);
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> func, TimeSpan? expire = null)
    {
        if (await ExistsAsync(key))
        {
            try
            {
                return await GetAsync<T>(key);
            }
            catch
            {
                await DelAsync(key);
            }
        }

        var result = await func.Invoke();

        if (expire.HasValue)
        {
            await SetAsync(key, result, expire.Value);
        }
        else
        {
            await SetAsync(key, result);
        }

        return result;
    }

    private List<string> GetAllKeys()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var coherentState = _memoryCache.GetType().GetField("_coherentState", flags).GetValue(_memoryCache);
        var entries = coherentState.GetType().GetField("_entries", flags).GetValue(coherentState);
        var cacheItems = entries as IDictionary;
        var keys = new List<string>();
        if (cacheItems == null) return keys;
        foreach (DictionaryEntry cacheItem in cacheItems)
        {
            keys.Add(cacheItem.Key.ToString());
        }
        return keys;
    }
}