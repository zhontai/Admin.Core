using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Services.User;

namespace ZhonTai.Admin.Core.Auth
{
    /// <summary>
    /// 权限处理
    /// </summary>
    public class PermissionHandler : IPermissionHandler
    {
        private readonly IUserService _userService;
        private readonly IUser _user;

        public PermissionHandler(IUserService userService, IUser user)
        {
            _userService = userService;
            _user = user;
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="api">接口路径</param>
        /// <param name="httpMethod">http请求方法</param>
        /// <returns></returns>
        public async Task<bool> ValidateAsync(string api, string httpMethod)
        {
            if (_user.PlatformAdmin)
            {
                return true;
            }

            var permissions = await _userService.GetPermissionsAsync();

            var valid = permissions.Any(m =>
                m.Path.NotNull() && m.Path.EqualsIgnoreCase($"/{api}")
                && m.HttpMethods.NotNull() && m.HttpMethods.Split(',').Any(n => n.NotNull() && n.EqualsIgnoreCase(httpMethod))
            );

            return valid;
        }
    }
}