using Admin.Core.Common.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Admin.Core.Attributes
{
    /// <summary>
    /// 输入模型验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ValidateInputAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                try
                {
                    var logger = (ILogger<ValidateInputAttribute>)context.HttpContext.RequestServices.GetService(typeof(ILogger<ValidateInputAttribute>));
                    var errorMessages = context.ModelState.Values.First().Errors.Select(a => a.ErrorMessage);
                    logger.LogError(string.Join(",", errorMessages));
                    context.Result = new JsonResult(ResponseOutput.NotOk(errorMessages.First()));
                }
                catch
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}