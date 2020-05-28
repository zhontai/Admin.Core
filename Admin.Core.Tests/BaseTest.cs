using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using NLog.Web;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Helpers;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace Admin.Core.Tests
{
    public class BaseTest
    {
        protected AppConfig AppConfig { get; }
        protected TestServer Server { get; }
        protected HttpClient Client { get; }
        protected IServiceProvider ServiceProvider { get; }

        protected BaseTest()
        {
            AppConfig = new ConfigHelper().Get<AppConfig>("appconfig") ?? new AppConfig();

            var builder = CreateHostBuilder();
            var host = builder.Build();
            host.Start();

            Server = host.GetTestServer();
            Client = host.GetTestClient();

            ServiceProvider = Server.Services;
        }

        private IHostBuilder CreateHostBuilder()
        {
            var configsPath = Path.Combine(AppContext.BaseDirectory, "configs").ToPath();
            

            return Host.CreateDefaultBuilder()
                  .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder
                      .UseEnvironment(EnvironmentName.Development)
                      .UseStartup<Startup>()
                      .ConfigureAppConfiguration((host, config) =>
                      {
                          if (AppConfig.RateLimit)
                          {
                              config.AddJsonFile($"{configsPath}/ratelimitconfig.json", optional: true, reloadOnChange: true)
#if DEBUG
                        .AddJsonFile($"{configsPath}/ratelimitconfig.Development.json", false)
#endif
                    ;
                          }
                      });
                      webBuilder.UseTestServer();
                  })
                  .ConfigureLogging(logging =>
                  {
                      logging.ClearProviders();
                      logging.SetMinimumLevel(LogLevel.Trace);
                  })
                  .UseNLog();
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
}
