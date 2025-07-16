using NLog.Web;
using System.Text;
using ZhonTai.IMServer.Host.Core.Configs;

var builder = WebApplication.CreateBuilder(args);

//清空日志供应程序，避免.net自带日志输出到命令台
builder.Logging.ClearProviders();
//使用NLog日志
builder.Host.UseNLog();

var imServerConfig = builder.Configuration.GetSection("ImServerConfig").Get<ImServerConfig>();
var healthChecks = imServerConfig.HealthChecks;
//添加健康检查
if (healthChecks != null && healthChecks.Enable)
{
    builder.Services.AddHealthChecks();
}

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.InputEncoding = Encoding.GetEncoding(imServerConfig.InputEncodingName);
Console.OutputEncoding = Encoding.GetEncoding(imServerConfig.OutputEncodingName);

app.UseFreeImServer(new ImServerOptions
{
    Redis = new FreeRedis.RedisClient(imServerConfig.RedisClientConnectionString),
    Servers = imServerConfig.Servers,
    Server = imServerConfig.Server,
});

app.Run();
