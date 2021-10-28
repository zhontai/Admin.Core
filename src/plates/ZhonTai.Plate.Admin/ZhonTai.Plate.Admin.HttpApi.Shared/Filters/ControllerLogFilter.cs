using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.HttpApi.Shared.Attributes;
using ZhonTai.Plate.Admin.HttpApi.Shared.Logs;

namespace ZhonTai.Plate.Admin.HttpApi.Shared.Filters
{
    /// <summary>
    /// 控制器操作日志记录
    /// </summary>
    public class ControllerLogFilter : IAsyncActionFilter
    {
        private readonly ILogHandler _logHandler;

        public ControllerLogFilter(ILogHandler logHandler)
        {
            _logHandler = logHandler;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(NoOprationLogAttribute)))
            {
                return next();
            }

            return _logHandler.LogAsync(context, next);
        }
    }
}