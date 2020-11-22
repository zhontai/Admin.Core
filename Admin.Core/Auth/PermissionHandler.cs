using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Service.Admin.User;

namespace Admin.Core.Auth
{
    /// <summary>
    /// 权限处理
    /// </summary>
    public class PermissionHandler : IPermissionHandler
    {
        private readonly IUserService _userService;

        public PermissionHandler(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="api">接口路径</param>
        /// <param name="httpMethod">http请求方法</param>
        /// <returns></returns>
        public async Task<bool> ValidateAsync(string api, string httpMethod)
        {
            var permissions = await _userService.GetPermissionsAsync();

            var valid = permissions.Any(m => 
                m.Path.NotNull() && m.Path.EqualsIgnoreCase($"/{api}")
                && m.HttpMethods.NotNull() && m.HttpMethods.Split(',').Any(n => n.NotNull() && n.EqualsIgnoreCase(httpMethod))
            );

            return valid;
        }
    }
}
