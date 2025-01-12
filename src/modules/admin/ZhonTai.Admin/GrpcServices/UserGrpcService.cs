using Mapster;
using ProtoBuf.Grpc;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.GrpcServices;
using ZhonTai.Admin.Core.GrpcServices.Dtos;
using ZhonTai.Admin.Core.Protos;
using ZhonTai.Admin.Services.User;

namespace ZhonTai.Admin.GrpcServices;

public class UserGrpcService : IUserGrpcService
{
    private readonly IUserService _userService;

    public UserGrpcService(IUserService userService)
    {
        _userService = userService;
    }

    public async Task GetDataPermissionAsync(ProtoString apiPath, CallContext context = default)
    {
        await _userService.GetDataPermissionAsync(apiPath);
    }

    public async Task<UserGetPermissionGrpcOutput> GetPermissionAsync(CallContext context = default)
    {
        var userPermission = await _userService.GetPermissionAsync();
        return userPermission.Adapt<UserGetPermissionGrpcOutput>();
    }
}
