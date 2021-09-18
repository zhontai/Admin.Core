using My.Admin.Common.Output;
using My.Admin.Service.Admin.Auth.Input;
using System.Threading.Tasks;

namespace My.Admin.Service.Admin.Auth
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