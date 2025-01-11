using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();
var configuration = app.Configuration;

// Configure the HTTP request pipeline.
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.OutputEncoding = Encoding.GetEncoding("GB2312");
Console.InputEncoding = Encoding.GetEncoding("GB2312");

app.UseFreeImServer(new ImServerOptions
{
    Redis = new FreeRedis.RedisClient(configuration["ImServerOptions:RedisClient"]),
    Servers = configuration["ImServerOptions:Servers"]?.Split(";"),
    Server = configuration["ImServerOptions:Server"],
});

app.Run();
