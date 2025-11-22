using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using ZhonTai.Common.Converters;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// Json帮助类
/// </summary>
public class JsonHelper
{
    // 线程安全的配置锁
    private static readonly Lock _configLock = new();
    private static readonly JsonSerializerOptions _jsonSerializerOptions = CreateDefaultOptions();

    /// <summary>
    /// 创建默认的JSON序列化选项
    /// </summary>
    private static JsonSerializerOptions CreateDefaultOptions()
    {
        return new JsonSerializerOptions
        {
            //大小写不敏感
            PropertyNameCaseInsensitive = true,
            //驼峰命名策略
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //忽略JSON注释	
            ReadCommentHandling = JsonCommentHandling.Skip,
            //忽略尾随逗号
            AllowTrailingCommas = true,
            //反序列化带引号的数字
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            //处理循环引用
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            //中文字符转义
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            //格式化输出
            WriteIndented = true,
            //元数据处理器
            TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
            //自定义转换器
            Converters =
            {
                //new StringNumberConverter(),
                new DateTimeConverter(),
                // 日期时间转换器
                new NullableDateTimeConverter(),
                // 弹性枚举转换器，支持字符串和数字两种方式
                new FlexibleEnumConverter(),
            },
        };
    }

    /// <summary>
    /// 获取当前JSON序列化选项的副本
    /// </summary>
    public static JsonSerializerOptions GetCurrentOptions()
    {
        using (_configLock.EnterScope())
        {
            return new JsonSerializerOptions(_jsonSerializerOptions);
        }
    }

    /// <summary>
    /// 配置默认的JSON序列化选项
    /// </summary>
    /// <param name="configure">配置委托</param>
    public static void ConfigureOptions(Action<JsonSerializerOptions> configure)
    {
        using (_configLock.EnterScope())
        {
            configure?.Invoke(_jsonSerializerOptions);
        }
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj,  _jsonSerializerOptions);
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static string Serialize<T>(T obj, JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(obj, options ?? _jsonSerializerOptions);
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static T Deserialize<T>(string json, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<T>(json, options ?? _jsonSerializerOptions);
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="json">json文本</param>
    /// <param name="type">类型</param>
    /// <returns></returns>
    public static object Deserialize(string json, Type type)
    {
        return JsonSerializer.Deserialize(json, type, _jsonSerializerOptions);
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="json">json文本</param>
    /// <param name="type">类型</param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static object Deserialize(string json, Type type, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize(json, type, options ?? _jsonSerializerOptions);
    }
}