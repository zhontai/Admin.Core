using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using ZhonTai.Admin.Core.Dto;

namespace ZhonTai.Admin.Core.Attributes;

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
                var errors = context.ModelState
               .Where(m => m.Value.ValidationState == ModelValidationState.Invalid)
               .Select(m =>
               {
                   var sb = new StringBuilder();
                   sb.AppendFormat("{0}：", m.Key);
                   sb.Append(m.Value.Errors.Select(n => n.ErrorMessage).Aggregate((x, y) => x + ";" + y));
                   return sb.ToString();
               })
               .Aggregate((x, y) => x + "|" + y);
                context.Result = new JsonResult(ResultOutput.NotOk(errors));
            }
            catch
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}