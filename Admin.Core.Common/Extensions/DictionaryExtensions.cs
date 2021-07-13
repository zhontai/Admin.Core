using System;
using System.Collections.Generic;

namespace Admin.Core.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(
           this IDictionary<TKey, TValue> dictionary,
           TKey key,
           Func<TKey, TValue> factory)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = factory(key);
                dictionary.Add(key, value);
            }
            return value;
        }

        public static TValue GetOrAdd<TKey, TValue>(
           this IDictionary<TKey, TValue> dictionary,
           TKey key,
           Func<TValue> factory)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = factory();
                dictionary.Add(key, value);
            }
            return value;
        }

        public static TValue AddOrUpdate<TKey, TValue>(
           this IDictionary<TKey, TValue> dictionary,
           TKey key,
           Func<TKey, TValue> addFactory,
           Func<TKey, TValue, TValue> updateFactory)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                value = updateFactory(key, value);
            }
            else
            {
                value = addFactory(key);
            }
            dictionary[key] = value;
            return value;
        }

        public static TValue AddOrUpdate<TKey, TValue>(
           this IDictionary<TKey, TValue> dictionary,
           TKey key,
           Func<TValue> addFactory,
           Func<TValue, TValue> updateFactory)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                value = updateFactory(value);
            }
            else
            {
                value = addFactory();
            }
            dictionary[key] = value;
            return value;
        }
    }
}