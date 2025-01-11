using ProtoBuf;

namespace ZhonTai.Admin.Core.Protos;

/// <summary>
/// ProtoString 表示 Grpc 请求或响应中的 string
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class ProtoString
{
    public string Value { get; set; }

    public ProtoString() { }

    public ProtoString(string value)
    {
        Value = value;
    }

    public static implicit operator ProtoString(string value)
    {
        return new ProtoString(value);
    }

    public static implicit operator string(ProtoString result)
    {
        return result.Value;
    }
}