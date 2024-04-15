using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core;

namespace MyApp.Tests;

/// <summary>
/// 测试基础
/// </summary>
public class BaseTest
{
    protected AppConfig AppConfig { get; }
    protected TestServer Server { get; }
    protected HttpClient Client { get; }
    protected IServiceProvider ServiceProvider { get; }

    protected BaseTest()
    {
        AppConfig = AppInfo.GetOptions<AppConfig>() ?? new AppConfig();
        var application = new WebApplicationFactory<Program>();
        Client = application.CreateClient();
        Server = application.Server;
        ServiceProvider = Server.Services;
    }

    public T GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }

    public T GetRequiredService<T>()
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}