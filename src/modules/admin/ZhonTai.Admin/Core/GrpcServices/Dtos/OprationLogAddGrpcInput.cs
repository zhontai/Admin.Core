using ProtoBuf;
using ZhonTai.Admin.Services.OperationLog.Dto;

namespace ZhonTai.Admin.Core.GrpcServices.Dtos;

/// <summary>
/// 操作日志
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class OperationLogAddGrpcInput: OperationLogAddInput
{

}
