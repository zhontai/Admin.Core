using System.Reflection;
using MyGateway.Host.Core.Configs;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

//清空日志供应程序，避免.net自带日志输出到命令台
builder.Logging.ClearProviders();
//使用NLog日志
builder.Host.UseNLog();

// 网关配置
var gatewayConfig = builder.Configuration.GetSection("GatewayConfig").Get<GatewayConfig>();
// 健康检查配置
var healthChecks = gatewayConfig?.HealthChecks;
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

//配置Kestrel服务器
//Kestrel配置
var kestrel = gatewayConfig?.Kestrel;
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(kestrel.KeepAliveTimeout);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(kestrel.RequestHeadersTimeout);
    options.Limits.MaxRequestBodySize = kestrel.MaxRequestBodySize;
});

//添加代理
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add services to the container.

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

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
        <link rel="icon" href="favicon.ico">
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