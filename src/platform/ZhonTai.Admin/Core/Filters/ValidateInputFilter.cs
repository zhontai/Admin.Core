using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;

namespace ZhonTai.Admin.Core.Filters;

/// <summary>
/// 输入模型验证过滤器
/// </summary>
public class ValidateInputFilter : IAsyncActionFilter
{
    public ValidateInputFilter()
    {
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            try
            {
                var logger = (ILogger<ValidateInputFilter>)context.HttpContext.RequestServices.GetService(typeof(ILogger<ValidateInputFilter>));
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
                logger.LogError(errors);
                context.Result = new JsonResult(ResultOutput.NotOk(errors));
            }
            catch
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return;
        }
        
        await next();
    }
}