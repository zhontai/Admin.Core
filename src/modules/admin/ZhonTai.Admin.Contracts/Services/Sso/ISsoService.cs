using ZhonTai.Admin.Contracts.Services.Sso.Dto;

namespace ZhonTai.Admin.Contracts.Services.Sso;

/// <summary>
/// 单点登录接口（一次性Ticket授权码方案）
/// </summary>
public interface ISsoService
{
    /// <summary>
    /// 生成票据
    /// </summary>
    Task<string> GenerateTicketAsync(SsoTicketInput input);

    /// <summary>
    /// 校验并消费票据
    /// </summary>
    Task<SsoUserInfo> ValidateTicketAsync(SsoValidateInput input);
}
