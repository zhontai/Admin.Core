using Mapster;
using ProtoBuf.Grpc;
using ZhonTai.Admin.Core.GrpcServices;
using ZhonTai.Admin.Core.GrpcServices.Dtos;
using ZhonTai.Admin.Core.Protos;
using ZhonTai.Admin.Services.User;

namespace ZhonTai.Admin.GrpcServices;

/// <summary>
/// 用户Grpc服务
/// </summary>
public class UserGrpcService : IUserGrpcService
{
    private readonly IUserService _userService;

    public UserGrpcService(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 获得数据权限
    /// </summary>
    /// <param name="apiPath"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task GetDataPermissionAsync(ProtoString apiPath, CallContext context = default)
    {
        await _userService.GetDataPermissionAsync(apiPath);
    }

    /// <summary>
    /// 获得用户权限
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<UserGetPermissionGrpcOutput> GetPermissionAsync(CallContext context = default)
    {
        var userPermission = await _userService.GetPermissionAsync();
        return userPermission.Adapt<UserGetPermissionGrpcOutput>();
    }
}
