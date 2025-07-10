using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core.Storage
{
    public interface IStorage
    {
        void Set<T>(string key, T value, DateTimeOffset absoluteExpiration);

        T Get<T>(string key);

        void Remove(string key);
    }
}
