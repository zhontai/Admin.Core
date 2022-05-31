using ZhonTai.Admin.Core.Dto;
using System.Threading.Tasks;
using ZhonTai.Admin.Services.Auth.Dto;
using ZhonTai.Admin.Tools.Captcha;

namespace ZhonTai.Admin.Services.Auth
{
    /// <summary>
    /// 认证授权接口
    /// </summary>
    public interface IAuthService
    {
        Task<IResultOutput> LoginAsync(AuthLoginInput input);

        Task<IResultOutput> GetUserInfoAsync();

        Task<IResultOutput> GetPasswordEncryptKeyAsync();

        Task<IResultOutput> GetCaptcha();

        Task<IResultOutput> CheckCaptcha(CaptchaInput input);

    }
}