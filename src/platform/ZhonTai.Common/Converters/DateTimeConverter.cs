using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZhonTai.Common.Converters;

/// <summary>
/// 日期时间转换器
/// </summary>
public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                // 返回默认时间（如 DateTime.MinValue）或根据需求抛出明确异常
                return default;
                // 或者抛出异常：throw new JsonException("无法将空字符串转换为 DateTime。");
            }
            if (DateTime.TryParse(stringValue, out var dateTime))
            {
                return dateTime;
            }
        }

        // 处理其他非字符串类型（如数字）
        try
        {
            return reader.GetDateTime();
        }
        catch (FormatException ex)
        {
            throw new JsonException("无效的 DateTime 格式。", ex);
        }
    }
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFFK"));
    }
}

/// <summary>
/// 可空日期时间转换器
/// </summary>
public class NullableDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString();

            // 处理空字符串或 null 字符串
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            // 尝试解析日期时间
            if (DateTime.TryParse(stringValue, out var dateTime))
            {
                return dateTime;
            }

            // 如果无法解析，抛出明确的异常
            throw new JsonException($"无法将值 '{stringValue}' 转换为 DateTime。");
        }

        // 处理其他类型（如数字时间戳）
        try
        {
            return reader.GetDateTime();
        }
        catch (Exception ex)
        {
            throw new JsonException("无效的 DateTime 格式。", ex);
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFFK"));
        }
    }
}