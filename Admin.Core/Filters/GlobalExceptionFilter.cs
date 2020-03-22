using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Admin.Core.Model.Output;
using Microsoft.Extensions.Logging;

namespace Admin.Core.Filters
{
    /// <summary>
    /// 全局异常错误过滤
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            string message;
            if (_env.IsProduction())
            {
                message = Enums.StatusCodes.Status500InternalServerError.ToDescriptionOrString();
            }
            else
            {
                message = context.Exception.Message;
            }

            _logger.LogError(context.Exception,"");

            var data = ResponseOutput.NotOk(message);

            context.Result = new InternalServerErrorResult(data);
        }
    }
    public class InternalServerErrorResult : ObjectResult
    {
        public InternalServerErrorResult(object value) : base(value)
        {
            StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError;
        }
    }

}
