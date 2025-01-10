using ProtoBuf;

namespace ZhonTai.Admin.Core.Protos;

/// <summary>
/// ProtoLong 表示 Grpc 请求或响应中的 long
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class ProtoLong
{
    public long Value { get; set; }

    public ProtoLong() { }

    public ProtoLong(long value)
    {
        Value = value;
    }

    public static implicit operator ProtoLong(long value)
    {
        return new ProtoLong(value);
    }

    public static implicit operator long(ProtoLong result)
    {
        return result.Value;
    }
}