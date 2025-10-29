using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZhonTai.Common.Converters;

/// <summary>
/// 字符串数字转换器
/// </summary>
public class StringNumberConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            // 处理各种数字类型
            if (reader.TryGetInt32(out int intValue))
                return intValue.ToString();
            if (reader.TryGetInt64(out long longValue))
                return longValue.ToString();
            if (reader.TryGetDouble(out double doubleValue))
                return doubleValue.ToString();
            if (reader.TryGetDecimal(out decimal decimalValue))
                return decimalValue.ToString();
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }

        // 如果是其他类型，转换为字符串
        return reader.GetString() ?? string.Empty;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}