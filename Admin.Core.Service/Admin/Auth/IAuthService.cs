using Admin.Core.Common.Output;
using Admin.Core.Service.Admin.Auth.Input;
using System.Threading.Tasks;

namespace Admin.Core.Service.Admin.Auth
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public interface IAuthService
    {
        Task<IResponseOutput> LoginAsync(AuthLoginInput input);

        Task<IResponseOutput> GetUserInfoAsync();

        Task<IResponseOutput> GetVerifyCodeAsync(string lastKey);

        Task<IResponseOutput> GetPassWordEncryptKeyAsync();
    }
}