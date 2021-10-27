using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.HttpApi.Shared.Attributes;
using ZhonTai.Plate.Admin.HttpApi.Shared.Logs;

namespace ZhonTai.Plate.Admin.HttpApi.Shared.Filters
{
    public class LogActionFilter : IAsyncActionFilter
    {
        private readonly ILogHandler _logHandler;

        public LogActionFilter(ILogHandler logHandler)
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