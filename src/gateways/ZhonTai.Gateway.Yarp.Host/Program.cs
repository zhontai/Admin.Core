using NLog.Web;
using System.Reflection;
using ZhonTai.Gateway.Yarp.Host.Core.Configs;

var builder = WebApplication.CreateBuilder(args);

//清空日志供应程序，避免.net自带日志输出到命令台
builder.Logging.ClearProviders();
//使用NLog日志
builder.Host.UseNLog();

var healthChecks = builder.Configuration.GetSection("GatewayConfig").Get<GatewayConfig>()?.HealthChecks;
//添加健康检查
if (healthChecks != null && healthChecks.Enable)
{
    builder.Services.AddHealthChecks();
}

//添加跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyPolicy", policy =>
    {
        policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

//添加代理
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

//使用跨域
app.UseCors("AllowAnyPolicy");

//使用健康检查
if (healthChecks != null && healthChecks.Enable)
{
    app.MapHealthChecks(healthChecks.Path);
}

//使用代理
app.MapReverseProxy();

//首页
app.MapGet("/", async (HttpResponse response) =>
{
    var gatewayConfig = builder.Configuration.GetSection("GatewayConfig").Get<GatewayConfig>();
    var moduleList = gatewayConfig?.ModuleList;
    var html = $"""
    <html>
    <head>
        <title>中台网关</title>
    </head>
    <body>
    """;
    if (moduleList?.Count > 0)
    {
        moduleList.ForEach(m =>
        {
            html += $"""<a href='{m.Url}' target="_blank">{m.Name}</a></br>""";
        });
    }
    else
    {
        html += $"The {Assembly.GetEntryAssembly()?.GetName().Name} has started.";
    }
    html += "</body></html>";

    response.ContentType = "text/html;charset=UTF-8";
    await response.WriteAsync(html);
});

app.Run();