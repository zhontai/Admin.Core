using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZhonTai.Plugin.Lazy.SlideCaptcha.Core.Converters;

/// <summary>
/// 日期时间转换器
/// </summary>
public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && DateTime.TryParse(reader.GetString(), out var dateTime))
        {
            return dateTime;
        }
        return reader.GetDateTime();
    }
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFFK"));
    }
}