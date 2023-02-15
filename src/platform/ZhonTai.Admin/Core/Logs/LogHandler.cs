using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using ZhonTai.Admin.Services.OprationLog;
using ZhonTai.Admin.Services.OprationLog.Dto;

namespace ZhonTai.Admin.Core.Logs;

/// <summary>
/// 操作日志处理
/// </summary>
public class LogHandler : ILogHandler
{
    private readonly ILogger _logger;
    private readonly ApiHelper _apiHelper;
    private readonly IOprationLogService _oprationLogService;

    public LogHandler(
        ILogger<LogHandler> logger,
        ApiHelper apiHelper,
        IOprationLogService oprationLogService
    )
    {
        _logger = logger;
        _apiHelper = apiHelper;
        _oprationLogService = oprationLogService;
    }

    public async Task LogAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var sw = new Stopwatch();
        sw.Start();
        var actionExecutedContext = await next();
        sw.Stop();

        //操作参数
        //var args = JsonConvert.SerializeObject(context.ActionArguments);
        //操作结果
        //var result = JsonConvert.SerializeObject(actionResult?.Value);

        try
        {
            var input = new OprationLogAddInput
            {
                Status = true,
                ApiMethod = context.HttpContext.Request.Method.ToLower(),
                ApiPath = context.ActionDescriptor.AttributeRouteInfo.Template.ToLower(),
                ElapsedMilliseconds = sw.ElapsedMilliseconds
            };

            if (actionExecutedContext.Exception != null)
            {
                input.Status = false;
                input.Msg = actionExecutedContext.Exception.Message;
            }

            input.ApiLabel = _apiHelper.GetApis().FirstOrDefault(a => a.Path == input.ApiPath)?.Label;

            await _oprationLogService.AddAsync(input);
        }
        catch (Exception ex)
        {
            _logger.LogError("操作日志插入异常：{@ex}", ex);
        }
    }
}