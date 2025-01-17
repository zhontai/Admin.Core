using ProtoBuf;
using System.Collections.Generic;

namespace ZhonTai.Admin.Core.Protos;

/// <summary>
/// ProtoList 表示 Grpc 请求或响应中的 List
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class ProtoList<T>
{
    public List<T> Value { get; set; }

    public ProtoList() { }

    public ProtoList(List<T> value)
    {
        Value = value;
    }

    public static implicit operator ProtoList<T>(List<T> value)
    {
        return new ProtoList<T>(value);
    }

    public static implicit operator List<T>(ProtoList<T> result)
    {
        return result.Value;
    }
}