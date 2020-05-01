using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Admin.OprationLog;
using Admin.Core.Service.Admin.OprationLog.Input;
using Microsoft.AspNetCore.Mvc.Filters;
//using Newtonsoft.Json;

namespace Admin.Core.Logs
{
    /// <summary>
    /// 操作日志处理
    /// </summary>
    public class LogHandler : ILogHandler
    {
        private readonly ApiHelper _apiHelper;
        private readonly IOprationLogService _oprationLogService;

        public LogHandler(
            ApiHelper apiHelper, 
            IOprationLogService oprationLogService
        )
        {
            _apiHelper = apiHelper;
            _oprationLogService = oprationLogService;
        }

        public async Task LogAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = new Stopwatch();
            sw.Start();

            dynamic actionResult = (await next()).Result;

            sw.Stop();

            //操作参数
            //var args = JsonConvert.SerializeObject(context.ActionArguments);
            //操作结果
            //var result = JsonConvert.SerializeObject(actionResult?.Value);

            var res = actionResult?.Value as IResponseOutput;

            var input = new OprationLogAddInput
            {
                ApiMethod = context.HttpContext.Request.Method.ToLower(),
                ApiPath = context.ActionDescriptor.AttributeRouteInfo.Template.ToLower(),
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = res?.Success,
                Msg = res?.Msg
            };
            input.ApiLabel = _apiHelper.GetApis().FirstOrDefault(a => a.Path == input.ApiPath)?.Label;

            await _oprationLogService.AddAsync(input);
        }
    }
}
