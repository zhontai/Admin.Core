using ProtoBuf;

namespace ZhonTai.Admin.Core.Protos;

/// <summary>
/// ProtoDecimal 表示 Grpc 请求或响应中的 decimal
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class ProtoDecimal
{
    public decimal Value { get; set; }

    public ProtoDecimal() { }

    public ProtoDecimal(decimal value)
    {
        Value = value;
    }

    public static implicit operator ProtoDecimal(decimal value)
    {
        return new ProtoDecimal(value);
    }

    public static implicit operator decimal(ProtoDecimal result)
    {
        return result.Value;
    }
}