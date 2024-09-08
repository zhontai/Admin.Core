using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace ZhonTai.Admin.Core.Logs;

/// <summary>
/// 操作日志处理接口
/// </summary>
public interface ILogHandler
{
    /// <summary>
    /// 写操作日志
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    Task LogAsync(ActionExecutingContext context, ActionExecutionDelegate next);
}