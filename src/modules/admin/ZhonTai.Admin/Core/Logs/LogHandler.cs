using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using ZhonTai.Admin.Core.Exceptions;
using ZhonTai.Admin.Services.OperationLog;
using ZhonTai.Admin.Services.OperationLog.Dto;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Dto;

namespace ZhonTai.Admin.Core.Logs;

/// <summary>
/// 操作日志处理
/// </summary>
public class LogHandler : ILogHandler
{
    private readonly ILogger _logger;
    private readonly ApiHelper _apiHelper;
    private readonly IOperationLogService _operationLogService;

    public LogHandler(
        ILogger<LogHandler> logger,
        ApiHelper apiHelper,
        IOperationLogService operationLogService
    )
    {
        _logger = logger;
        _apiHelper = apiHelper;
        _operationLogService = operationLogService;
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

            //操作参数
            if (api.EnabledParams && context.ActionArguments.Count > 0) 
            {
                input.Params = JsonHelper.Serialize(context.ActionArguments);
            }

            //操作结果
            if (api.EnabledResult && actionExecutedContext.Result != null && actionExecutedContext.Result is JsonResult result)
            {
                input.Result = JsonHelper.Serialize(result.Value);
            }

            if (actionExecutedContext.Exception != null)
            {
                input.Status = false;

                var code = "";
                if(actionExecutedContext.Exception is AppException appException)
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
                }.NotOk(actionExecutedContext.Exception.Message));
            }
            


            input.ApiLabel = api?.Label;

            await _operationLogService.AddAsync(input);
        }
        catch (Exception ex)
        {
            _logger.LogError("操作日志插入异常：{@ex}", ex);
        }
    }
}