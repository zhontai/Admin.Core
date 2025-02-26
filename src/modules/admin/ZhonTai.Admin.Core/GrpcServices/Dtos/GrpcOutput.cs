using ProtoBuf;

namespace ZhonTai.Admin.Core.GrpcServices.Dtos;

/// <summary>
/// Grpc输出
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.None)]
public class GrpcOutput<T>
{
    /// <summary>
    /// 是否成功标记
    /// </summary>
    [ProtoMember(1)]
    public bool Success { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [ProtoMember(2)]
    public string Code { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    [ProtoMember(3)]
    public string Msg { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    [ProtoMember(4)]
    public T Data { get; set; }
}