using ProtoBuf;

namespace ZhonTai.Admin.Core.Dto;

/// <summary>
/// Grpc输出
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class GrpcOutput<T> : ResultOutput<T>
{

}