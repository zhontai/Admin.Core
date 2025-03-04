using Microsoft.AspNetCore.Mvc.ModelBinding;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Services.Auth.Dto;

namespace ZhonTai.Admin.Services.Auth;

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