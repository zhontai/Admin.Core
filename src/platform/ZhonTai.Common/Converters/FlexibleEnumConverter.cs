using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZhonTai.Common.Converters;

/// <summary>
/// 弹性枚举转换器，支持字符串和数字两种方式
/// </summary>
public class FlexibleEnumConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    {
        return (JsonConverter)Activator.CreateInstance(typeof(EnumConverter<>).MakeGenericType(type));
    }

    private class EnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        // 获取枚举的基础类型
        private readonly Type _underlyingType = Enum.GetUnderlyingType(typeof(T));

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonTokenType tokenType = reader.TokenType;

            // 处理字符串类型的值
            if (tokenType == JsonTokenType.String)
            {
                string enumString = reader.GetString();
                if (Enum.TryParse(enumString, true, out T value))
                    return value;
            }
            // 处理数字类型的值
            else if (tokenType == JsonTokenType.Number)
            {
                try
                {
                    // 根据基础类型动态解析数字值
                    object numericValue = _underlyingType.Name switch
                    {
                        nameof(Int32) => reader.GetInt32(),
                        nameof(UInt32) => reader.GetUInt32(),
                        nameof(Int64) => reader.GetInt64(),
                        nameof(UInt64) => reader.GetUInt64(),
                        nameof(Int16) => (short)reader.GetInt32(),
                        nameof(UInt16) => (ushort)reader.GetUInt32(),
                        nameof(Byte) => (byte)reader.GetUInt32(),
                        nameof(SByte) => (sbyte)reader.GetInt32(),
                        _ => throw new JsonException($"Unsupported underlying type: {_underlyingType}")
                    };

                    return (T)Enum.ToObject(typeof(T), numericValue);
                }
                catch
                {
                    // 数字转换失败时尝试字符串回退
                    string stringValue = reader.TryGetInt64(out long longVal)
                        ? longVal.ToString()
                        : reader.GetDouble().ToString();

                    if (Enum.TryParse(stringValue, true, out T value))
                        return value;
                }
            }

            throw new JsonException($"Unable to convert value to {typeof(T).Name}");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // 直接写入数字值（根据实际基础类型）
            switch (Convert.GetTypeCode(Enum.ToObject(typeof(T), value)))
            {
                case TypeCode.Byte:
                    writer.WriteNumberValue(Convert.ToByte(value));
                    break;
                case TypeCode.SByte:
                    writer.WriteNumberValue(Convert.ToSByte(value));
                    break;
                case TypeCode.Int16:
                    writer.WriteNumberValue(Convert.ToInt16(value));
                    break;
                case TypeCode.UInt16:
                    writer.WriteNumberValue(Convert.ToUInt16(value));
                    break;
                case TypeCode.Int32:
                    writer.WriteNumberValue(Convert.ToInt32(value));
                    break;
                case TypeCode.UInt32:
                    writer.WriteNumberValue(Convert.ToUInt32(value));
                    break;
                case TypeCode.Int64:
                    writer.WriteNumberValue(Convert.ToInt64(value));
                    break;
                case TypeCode.UInt64:
                    writer.WriteNumberValue(Convert.ToUInt64(value));
                    break;
                default:
                    writer.WriteStringValue(value.ToString());
                    break;
            }
        }
    }
}