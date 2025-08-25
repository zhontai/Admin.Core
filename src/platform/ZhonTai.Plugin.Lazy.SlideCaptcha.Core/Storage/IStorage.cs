namespace ZhonTai.Plugin.Lazy.SlideCaptcha.Core.Storage;

public interface IStorage
{
    void Set<T>(string key, T value, DateTimeOffset absoluteExpiration);

    T Get<T>(string key);

    void Remove(string key);
}
