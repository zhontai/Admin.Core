using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZhonTai.Common.Converters;

/// <summary>
/// 可空字符串转换器，允许非字符串值（数字、布尔等）反序列化为可空字符串属性
/// </summary>
public class StringNullableConverter : JsonConverter<string?>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.True => "true",
            JsonTokenType.False => "false",
            JsonTokenType.Number => ReadNumber(ref reader),
            _ => string.Empty
        };
    }

    private static string? ReadNumber(ref Utf8JsonReader reader)
    {
        if (reader.TryGetInt64(out long longValue))
            return longValue.ToString();
        if (reader.TryGetDecimal(out decimal decimalValue))
            return decimalValue.ToString();
        return reader.TryGetDouble(out double doubleValue) ? doubleValue.ToString() : string.Empty;
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}

/// <summary>
/// 字符串转换器，允许非字符串值（数字、布尔等）反序列化为字符串属性
/// </summary>
public class StringConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null or JsonTokenType.None => string.Empty,
            JsonTokenType.String => reader.GetString() ?? string.Empty,
            JsonTokenType.True => "true",
            JsonTokenType.False => "false",
            JsonTokenType.Number => ReadNumber(ref reader),
            _ => string.Empty
        };
    }

    private static string ReadNumber(ref Utf8JsonReader reader)
    {
        if (reader.TryGetInt64(out long longValue))
            return longValue.ToString();
        if (reader.TryGetDecimal(out decimal decimalValue))
            return decimalValue.ToString();
        return reader.TryGetDouble(out double doubleValue) ? doubleValue.ToString() : string.Empty;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
