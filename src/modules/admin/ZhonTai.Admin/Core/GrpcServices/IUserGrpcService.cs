using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.GrpcServices.Dtos;
using ZhonTai.Admin.Core.Protos;

namespace ZhonTai.Admin.Core.GrpcServices;

/// <summary>
/// 用户服务接口
/// </summary>
[ServiceContract(ConfigurationName = AdminConsts.AreaName)]
public interface IUserGrpcService
{
    Task GetDataPermissionAsync(ProtoString apiPath, CallContext context = default);

    Task<UserGetPermissionGrpcOutput> GetPermissionAsync(CallContext context = default);
}
