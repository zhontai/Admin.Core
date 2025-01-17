using ProtoBuf;

namespace ZhonTai.Admin.Core.Protos;

/// <summary>
/// ProtoInt 表示 Grpc 请求或响应中的 int
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class ProtoInt
{
    public int Value { get; set; }

    public ProtoInt() { }

    public ProtoInt(int value)
    {
        Value = value;
    }

    public static implicit operator ProtoInt(int value)
    {
        return new ProtoInt(value);
    }

    public static implicit operator long(ProtoInt result)
    {
        return result.Value;
    }
}