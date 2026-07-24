using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZhonTai.Common.Converters;

/// <summary>
/// 字符串转换器，允许非字符串值（数字、布尔等）反序列化为字符串属性
/// </summary>
public class StringConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                if (reader.TryGetDecimal(out decimal decimalValue))
                    return decimalValue.ToString();
                if (reader.TryGetInt64(out long longValue))
                    return longValue.ToString();
                if (reader.TryGetDouble(out double doubleValue))
                    return doubleValue.ToString();
                return string.Empty;

            case JsonTokenType.String:
                return reader.GetString() ?? string.Empty;

            case JsonTokenType.True:
                return "true";

            case JsonTokenType.False:
                return "false";

            case JsonTokenType.Null:
                return string.Empty;

            default:
                return string.Empty;
        }
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}