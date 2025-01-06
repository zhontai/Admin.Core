using NLog.Web;
using ZhonTai.Gateway.Yarp.Core.Configs;

var builder = WebApplication.CreateBuilder(args);

//清空日志供应程序，避免.net自带日志输出到命令台
builder.Logging.ClearProviders();
//使用NLog日志
builder.Host.UseNLog();

builder.Services.AddHealthChecks();

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

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors("AllowAnyPolicy");

app.MapHealthChecks("/health");

app.MapReverseProxy();

//app.MapGet("/", () => $"The {Assembly.GetEntryAssembly()?.GetName().Name} has started.");

app.MapGet("/", async (HttpResponse response) =>
{
    var gatewayConfig = builder.Configuration.GetSection("GatewayConfig").Get<GatewayConfig>();

    var moduleList = gatewayConfig.ModuleList;

    var html = $"<html><body>";
    if (moduleList?.Count > 0)
    {
        moduleList.ForEach(m =>
        {
            html += $"""<a href='{m.Url}' target="_blank">{m.Name}</a></br>""";
        });
    }
    else
    {
        html += "MyGateway.Host start!";
    }
    html += "</body></html>";

    response.ContentType = "text/html;charset=UTF-8";

    await response.WriteAsync(html);
});


app.Run();