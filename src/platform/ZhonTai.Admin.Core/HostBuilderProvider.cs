using Autofac.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using ZhonTai.Common.Configs;
using ZhonTai.Common.Helpers;

namespace ZhonTai.Admin.Core
{
    public class HostBuilderProvider
    {
        private AppConfig appConfig = new ConfigHelper().Get<AppConfig>("appconfig") ?? new AppConfig();

        public void Run<T>(string[] args) where T : BaseStartup
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                Console.WriteLine(" launching...");
                var host = CreateHostBuilder<T>(args).Build();
                Console.WriteLine($"\r\n {string.Join("\r\n ", appConfig.Urls)}\r\n");
                host.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public IHostBuilder CreateHostBuilder<T>(string[] args) where T : BaseStartup
        {
            //使用logconfig.json配置，默认使用nlog.config
            //var logConfig = new ConfigHelper().Load("logconfig", reloadOnChange: true).GetSection("nLog");
            //LogManager.Configuration = new NLogLoggingConfiguration(logConfig);

            return Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                //.UseEnvironment(Environments.Production)
                .UseStartup<T>()
                .ConfigureAppConfiguration((host, config) =>
                {
                    if (appConfig.RateLimit)
                    {
                        config.AddJsonFile("./configs/ratelimitconfig.json", optional: false, reloadOnChange: true);
                        if (host.HostingEnvironment.EnvironmentName.NotNull())
                        {
                            config.AddJsonFile($"./configs/ratelimitconfig.{host.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                        }
                    }
                })
                .UseUrls(appConfig.Urls);
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .UseNLog();
        }
    }
}
