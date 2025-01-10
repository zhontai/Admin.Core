using ProtoBuf;

namespace ZhonTai.Admin.Core.Protos;

/// <summary>
/// ProtoBoolean 表示 Grpc 请求或响应中的 bool
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class ProtoBoolean
{
    public bool Value { get; set; }

    public ProtoBoolean() { }

    public ProtoBoolean(bool value)
    {
        Value = value;
    }

    public static implicit operator ProtoBoolean(bool value)
    {
        return new ProtoBoolean(value);
    }

    public static implicit operator bool(ProtoBoolean result)
    {
        return result.Value;
    }
}