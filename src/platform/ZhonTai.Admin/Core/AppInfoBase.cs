using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ZhonTai.Admin.Core;

internal static class AppInfoBase
{
    internal static IServiceCollection Services;

    internal static IServiceProvider ServiceProvider;

    internal static IWebHostEnvironment WebHostEnvironment;

    internal static IHostEnvironment HostEnvironment;

    internal static IConfiguration Configuration;

    internal static void ConfigureApplication(this WebApplicationBuilder webApplicationBuilder)
    {
        HostEnvironment = webApplicationBuilder.Environment;
        WebHostEnvironment = webApplicationBuilder.Environment;
        Services = webApplicationBuilder.Services;
        Configuration = webApplicationBuilder.Configuration;
    }

    internal static void ConfigureApplication(this WebApplication app)
    {
        ServiceProvider = app.Services;
    }
}
