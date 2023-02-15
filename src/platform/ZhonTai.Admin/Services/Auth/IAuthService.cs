using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using ZhonTai.Admin.Services.Auth.Dto;
using ZhonTai.Admin.Tools.Captcha;

namespace ZhonTai.Admin.Services.Auth;

/// <summary>
/// 认证授权接口
/// </summary>
public interface IAuthService
{
    Task<dynamic> LoginAsync(AuthLoginInput input);

    Task<AuthGetUserInfoOutput> GetUserInfoAsync();

    Task<AuthGetPasswordEncryptKeyOutput> GetPasswordEncryptKeyAsync();

    Task<CaptchaOutput> GetCaptcha();

    Task CheckCaptcha(CaptchaInput input);

    Task<dynamic> Refresh([BindRequired] string token);

}