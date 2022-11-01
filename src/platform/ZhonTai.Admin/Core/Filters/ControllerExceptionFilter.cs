using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Exceptions;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Admin.Core.Filters;

/// <summary>
/// 控制器异常处理
/// </summary>
public class ControllerExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ControllerExceptionFilter> _logger;

    public ControllerExceptionFilter(IWebHostEnvironment env, ILogger<ControllerExceptionFilter> logger)
    {
        _env = env;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled == false)
        {
            string message;
            var appException = context.Exception is AppException;
            if (_env.IsProduction())
            {
                message = appException ? context.Exception.Message : Enums.StatusCodes.Status500InternalServerError.ToDescription();
            }
            else
            {
                message = context.Exception.Message;
            }

            if (!appException)
            {
                _logger.LogError(context.Exception, "");
            }
            
            var data = ResultOutput.NotOk(message);
            context.Result = new InternalServerErrorResult(data, appException);
        }

        context.ExceptionHandled = true;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        OnException(context);
        return Task.CompletedTask;
    }
}

public class InternalServerErrorResult : ObjectResult
{
    public InternalServerErrorResult(object value, bool appException) : base(value)
    {
        StatusCode = appException ? Microsoft.AspNetCore.Http.StatusCodes.Status200OK : Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError;
    }
}