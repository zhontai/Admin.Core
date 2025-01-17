using Microsoft.AspNetCore.Mvc.ModelBinding;
using Refit;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Services.Auth.Dto;

namespace ZhonTai.Admin.Services.Auth;

/// <summary>
/// 认证授权客户端接口
/// </summary>
[HttpClientContract(AdminConsts.AreaName)]
public interface IAuthClientService
{
    [Post("/api/admin/auth/login")]
    Task<TokenInfo> LoginAsync(AuthLoginInput input);
}

/// <summary>
/// 认证授权接口
/// </summary>
public interface IAuthService: IAuthClientService
{
    string GetToken(AuthLoginOutput user);

    TokenInfo GetTokenInfo(AuthLoginOutput user);

    Task<AuthGetUserInfoOutput> GetUserInfoAsync();

    Task<AuthGetPasswordEncryptKeyOutput> GetPasswordEncryptKeyAsync();

    Task<TokenInfo> Refresh([BindRequired] string token);

}