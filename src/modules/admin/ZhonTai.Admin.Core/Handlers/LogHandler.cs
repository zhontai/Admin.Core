using DotNetCore.CAP;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Exceptions;
using ZhonTai.Admin.Core.GrpcServices;
using ZhonTai.Admin.Core.GrpcServices.Dtos;
using ZhonTai.Admin.Core.Helpers;
using ZhonTai.Admin.Services.Api.Dto;
using ZhonTai.Common.Helpers;

namespace ZhonTai.Admin.Core.Handlers;

/// <summary>
/// 操作日志处理
/// </summary>
public class LogHandler : ILogHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger _logger;
    private readonly IOptions<AppConfig> _appConfig;
    private readonly IOprationLogGrpcService _oprationLogGrpcService;
    private readonly IUser _user;
    private readonly ApiHelper _apiHelper;

    public LogHandler(
        IHttpContextAccessor httpContextAccessor,
        ILogger<LogHandler> logger,
        IOptions<AppConfig> appConfig,
        IOprationLogGrpcService oprationLogGrpcService,
        IUser user,
        ApiHelper apiHelper
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _appConfig = appConfig;
        _oprationLogGrpcService = oprationLogGrpcService;
        _user = user;
        _apiHelper = apiHelper;
    }

    public async Task LogAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        var actionExecutedContext = await next();
        stopwatch.Stop();

        try
        {
            var input = new OperationLogAddInput
            {
                Status = true,
                ApiMethod = context.HttpContext.Request.Method.ToLower(),
                ApiPath = context.ActionDescriptor.AttributeRouteInfo.Template.ToLower(),
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                StatusCode = context.HttpContext.Response.StatusCode
            };

            var api = (await _apiHelper.GetApiListAsync()).FirstOrDefault(a => a.Path == input.ApiPath);
            //操作日志启用
            if ((api != null && api.EnabledLog))
            {
                var excepton = actionExecutedContext.Exception;

                //操作参数
                if ((api != null && api.EnabledParams && context.ActionArguments.Count > 0) || excepton != null)
                {
                    input.Params = JsonHelper.Serialize(context.ActionArguments);
                }

                //操作结果
                if (api != null && api.EnabledResult && actionExecutedContext.Result != null && actionExecutedContext.Result is JsonResult result)
                {
                    input.Result = JsonHelper.Serialize(result.Value);
                }

                if (excepton != null)
                {
                    input.Status = false;

                    var code = "";
                    if (excepton is AppException appException)
                    {
                        input.StatusCode = appException.StatusCode;
                        code = appException.AppCode;
                    }
                    else
                    {
                        input.StatusCode = (int)HttpStatusCode.InternalServerError;
                    }

                    input.Result = JsonHelper.Serialize(new ResultOutput<string>()
                    {
                        Code = code
                    }.NotOk(excepton.Message));
                }

                input.ApiLabel = api?.Label;

                if (input.IP.IsNull())
                {
                    input.IP = IPHelper.GetIP(_httpContextAccessor?.HttpContext?.Request);
                }

                string ua = _httpContextAccessor?.HttpContext?.Request?.Headers?.UserAgent;
                if (ua.NotNull())
                {
                    input.BrowserInfo = ua;
                }

                input.Name = _user.Name;
                input.CreatedUserId = _user.Id;
                input.CreatedUserName = _user.UserName;
                input.CreatedUserRealName = _user.Name;
                input.TenantId = _user.TenantId;

                if(_appConfig.Value.Log.Method == LogMethod.Cap)
                {
                    await AppInfo.GetRequiredService<ICapPublisher>().PublishAsync(SubscribeNames.OperationLogAdd, input);
                }
                else
                {
                    await _oprationLogGrpcService.AddAsync(input.Adapt<OperationLogAddGrpcInput>());
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("操作日志插入异常：{@ex}", ex);
        }
    }
}