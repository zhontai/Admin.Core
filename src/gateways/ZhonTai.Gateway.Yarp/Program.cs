using NLog.Web;

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
    var serviceList = new List<Tuple<string, string>>
    {
        new("权限接口文档","/doc/admin/index.html")
    };

    var html = $"<html><body>";

    serviceList.ForEach(m =>
    {
        html += $"""<a href='{m.Item2}' target="_blank">{m.Item1}</a></br>""";
    });

    html += "</body></html>";

    response.ContentType = "text/html;charset=UTF-8";

    await response.WriteAsync(html);
});


app.Run();