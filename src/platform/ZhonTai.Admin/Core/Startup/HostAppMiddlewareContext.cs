using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ZhonTai.Admin.Core.Startup;

/// <summary>
/// HostApp中间件上下文
/// </summary>
public class HostAppMiddlewareContext
{
    /// <summary>
    /// 应用
    /// </summary>
    public WebApplication App { get; set; }

    /// <summary>
    /// 环境
    /// </summary>
    public IHostEnvironment Environment { get; set; }

    /// <summary>
    /// 配置
    /// </summary>
    public IConfiguration Configuration { get; set; }
}

