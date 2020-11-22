using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;
using Autofac.Extensions.DependencyInjection;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Configs;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using System.Threading.Tasks;
//using NLog.Extensions.Logging;

namespace Admin.Core
{
    public class Program
    {
        private static AppConfig appConfig = new ConfigHelper().Get<AppConfig>("appconfig") ?? new AppConfig();
        public static async Task<int> Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                Console.WriteLine(" launching...");
                var host = CreateHostBuilder(args).Build();
                Console.WriteLine($"\r\n {string.Join("\r\n ", appConfig.Urls)}\r\n");
                await host.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                return 1;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
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
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((host, config) =>
                {
                    if (appConfig.RateLimit)
                    {
                        config.AddJsonFile("./configs/ratelimitconfig.json", optional: false, reloadOnChange: true)
#if DEBUG
                        .AddJsonFile("./configs/ratelimitconfig.Development.json", true)
#endif
                    ;
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
