using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace ZhonTai.Admin.Core.Auth;

/// <summary>
/// 自定义权限处理接口
/// </summary>
public interface ICustomPermissionHandler
{
    /// <summary>
    /// 权限验证
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<bool> ValidateAsync(AuthorizationFilterContext context);
}