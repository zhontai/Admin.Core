using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Attributes;
using Admin.Core.Service.Admin.User;

namespace Admin.Core.Auth
{
    /// <summary>
    /// 权限处理
    /// </summary>
   [SingleInstance]
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

            //var isValid = permissions.Any(m => m.EqualsIgnoreCase($"{httpMethod}/{api}"));
            var isValid = permissions.Any(m => m != null && m.EqualsIgnoreCase($"/{api}"));
            return isValid;
        }
    }
}
