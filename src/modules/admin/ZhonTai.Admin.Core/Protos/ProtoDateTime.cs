using ProtoBuf;
using System;

namespace ZhonTai.Admin.Core.Protos;

/// <summary>
/// ProtoDateTime 表示 Grpc 请求或响应中的 DateTime
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class ProtoDateTime
{
    public DateTime Value { get; set; }

    public ProtoDateTime() { }

    public ProtoDateTime(DateTime value)
    {
        Value = value;
    }

    public static implicit operator ProtoDateTime(DateTime value)
    {
        return new ProtoDateTime(value);
    }

    public static implicit operator DateTime(ProtoDateTime result)
    {
        return result.Value;
    }
}