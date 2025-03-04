using ProtoBuf.Grpc;
using System.ServiceModel;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.GrpcServices.Dtos;
using ZhonTai.Admin.Core.Protos;

namespace ZhonTai.Admin.Core.GrpcServices;

/// <summary>
/// 操作日志服务接口
/// </summary>
[ServiceContract(ConfigurationName = AdminConsts.AreaName)]
public interface IOprationLogGrpcService
{
    Task<ProtoLong> AddAsync(OperationLogAddGrpcInput input, CallContext context = default);
}
