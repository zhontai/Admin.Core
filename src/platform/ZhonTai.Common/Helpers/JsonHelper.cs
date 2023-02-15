using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// Json帮助类
/// </summary>
public class JsonHelper
{
    private static readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        DateFormatString = "yyyy-MM-dd HH:mm:ss"
};

    /// <summary>
    /// 序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, typeof(T), _jsonSerializerSettings);
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings);
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="json">json文本</param>
    /// <param name="type">类型</param>
    /// <returns></returns>
    public static object Deserialize(string json, Type type)
    {
        return JsonConvert.DeserializeObject(json, type, _jsonSerializerSettings);
    }
}