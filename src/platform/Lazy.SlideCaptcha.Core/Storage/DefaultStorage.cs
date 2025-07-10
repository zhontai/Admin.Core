using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core.Storage
{
    public class DefaultStorage : IStorage
    {
        private readonly IDistributedCache _cache;
        private readonly IOptionsMonitor<CaptchaOptions> _options;

        public DefaultStorage(IOptionsMonitor<CaptchaOptions> options, IDistributedCache cache)
        {
            _options = options;
            _cache = cache;
        }

        private string WrapKey(string key)
        {
            return $"{this._options.CurrentValue.StoreageKeyPrefix}{key}";
        }

        public T Get<T>(string key)
        {
            var bytes = _cache.Get(WrapKey(key));
            if (bytes == null) return default(T);
            var json = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Remove(string key)
        {
            _cache.Remove(WrapKey(key));
        }

        public void Set<T>(string key, T value, DateTimeOffset absoluteExpiration)
        {
            string json = JsonConvert.SerializeObject(value);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            _cache.Set(WrapKey(key), bytes, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = absoluteExpiration
            });
        }
    }
}
