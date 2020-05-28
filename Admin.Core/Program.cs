using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Autofac.Extensions.DependencyInjection;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Configs;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using Microsoft.Extensions.Configuration;
//using NLog;
//using NLog.Extensions.Logging;
//using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace Admin.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("launching...");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            //使用logconfig.json配置，默认使用nlog.config
            //var logConfig = new ConfigHelper().Load("logconfig", reloadOnChange: true).GetSection("nLog");
            //LogManager.Configuration = new NLogLoggingConfiguration(logConfig);

            var appConfig = new ConfigHelper().Get<AppConfig>("appconfig") ?? new AppConfig();

            return Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                //.UseEnvironment(EnvironmentName.Production)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((host, config) =>
                {
                    if (appConfig.RateLimit)
                    {
                        config.AddJsonFile("./configs/ratelimitconfig.json", optional: true, reloadOnChange: true)
#if DEBUG
                        .AddJsonFile("./configs/ratelimitconfig.Development.json", false)
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
