using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.GrpcServices;
using ZhonTai.Admin.Core.Handlers;

namespace ZhonTai.Admin.Core.Auth;

/// <summary>
/// 权限处理
/// </summary>
public class PermissionHandler : IPermissionHandler
{
    private readonly IUser _user;
    private readonly IUserGrpcService _userGrpcService;

    public PermissionHandler(IUser user, IUserGrpcService userGrpcService)
    {
        _user = user;
        _userGrpcService = userGrpcService;
    }

    /// <summary>
    /// 权限验证
    /// </summary>
    /// <param name="api">接口路径</param>
    /// <param name="httpMethod">http请求方法</param>
    /// <param name="apiAccess">接口访问</param>
    /// <returns></returns>
    public async Task<bool> ValidateAsync(string api, string httpMethod, ApiAccessAttribute apiAccess)
    {
        if (_user.PlatformAdmin)
        {
            return true;
        }

        var userPermission = await _userGrpcService.GetPermissionAsync();

        var valid = userPermission.Apis.Any(m =>
            m.Path.NotNull() && m.Path.EqualsIgnoreCase($"/{api}")
            && m.HttpMethods.NotNull() && m.HttpMethods.Split(',').Any(n => n.NotNull() && n.EqualsIgnoreCase(httpMethod))
        );

        if (!valid && apiAccess != null) 
        {
            if (apiAccess.All)
            {
                valid = userPermission.Codes.All(a => apiAccess.Codes.Contains(a));
            }
            else
            {
                valid = userPermission.Codes.Any(a => apiAccess.Codes.Contains(a));
            }
        }

        return valid;
    }
}