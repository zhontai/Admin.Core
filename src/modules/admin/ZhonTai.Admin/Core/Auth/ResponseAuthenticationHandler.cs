using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ZhonTai.Common.Extensions;
using StatusCodes = ZhonTai.Admin.Core.Enums.StatusCodes;

namespace ZhonTai.Admin.Core.Auth;

/// <summary>
/// 响应认证处理器
/// </summary>
public class ResponseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
#if NET8_0_OR_GREATER
    public ResponseAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder
    ) : base(options, logger, encoder) 
#else
    public ResponseAuthenticationHandler(
       IOptionsMonitor<AuthenticationSchemeOptions> options,
       ILoggerFactory logger,
       UrlEncoder encoder,
       ISystemClock systemClock
   ) : base(options, logger, encoder, systemClock)
#endif
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException();
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.ContentType = "application/json";
        Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized;
        await Response.WriteAsync(JsonConvert.SerializeObject(
            new ResponseStatusData
            {
                Code = StatusCodes.Status401Unauthorized,
                Msg = StatusCodes.Status401Unauthorized.ToDescription(),
                Success = false
            },
            new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        ));
    }

    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.ContentType = "application/json";
        Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden;
        await Response.WriteAsync(JsonConvert.SerializeObject(
            new ResponseStatusData
            {
                Code = StatusCodes.Status403Forbidden,
                Msg = StatusCodes.Status403Forbidden.ToDescription(),
                Success = false
            },
            new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        ));
    }
}

public class ResponseStatusData
{
    public StatusCodes Code { get; set; } = StatusCodes.Status1Ok;
    public string Msg { get; set; }
    public bool Success { get; set; }

}