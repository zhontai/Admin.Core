using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.GrpcServices.Dtos;

namespace ZhonTai.Admin.Core.GrpcServices;

/// <summary>
/// Api服务接口
/// </summary>
[ServiceContract(ConfigurationName = AdminConsts.AreaName)]
public interface IApiGrpcService
{
    Task<GrpcOutput<List<ApiGrpcOutput>>> GetApiList(CallContext context = default);
}
