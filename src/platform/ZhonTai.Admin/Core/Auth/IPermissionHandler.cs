using System.Threading.Tasks;

namespace ZhonTai.Admin.Core.Auth;

/// <summary>
/// 权限处理接口
/// </summary>
public interface IPermissionHandler
{
    /// <summary>
    /// 权限验证
    /// </summary>
    /// <param name="api"></param>
    /// <param name="httpMethod"></param>
    /// <returns></returns>
    Task<bool> ValidateAsync(string api, string httpMethod);
}