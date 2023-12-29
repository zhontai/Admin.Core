using Microsoft.Extensions.DependencyInjection;
#if (!NoApiUI)
using Microsoft.Extensions.Hosting;
#endif
using ZhonTai;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Startup;
#if (!NoTaskScheduler)
using ZhonTai.Admin.Tools.TaskScheduler;
#endif
#if (!NoApiUI)
using ZhonTai.ApiUI;
#endif
using ZhonTai.Common.Helpers;
using MyApp.Api.Core.Consts;
using Microsoft.AspNetCore.Builder;
#if (!NoCap)
using Microsoft.Extensions.DependencyModel;
using Savorboard.CAP.InMemoryMessageQueue;
using System.Reflection;
using System.Linq;
#endif
#if (!NoTaskScheduler)
using FreeScheduler;
#endif
using AdminDbkeys = ZhonTai.Admin.Core.Consts.DbKeys;
using AdminSubscribeNames = ZhonTai.Admin.Core.Consts.SubscribeNames;
#if (!NoTaskScheduler)
using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Domain;
using System;
using Cronos;
#endif
#if (!NoTaskScheduler)

static void ConfigureScheduler(IFreeSql fsql)
{
    fsql.CodeFirst
    .ConfigEntity<TaskInfo>(a =>
    {
        a.Name("app_task");
    })
    .ConfigEntity<TaskLog>(a =>
    {
        a.Name("app_task_log");
    })
    .ConfigEntity<TaskInfoExt>(a =>
    {
        a.Name("app_task_ext");
    });
}
#endif

new HostApp(new HostAppOptions()
{
    //配置FreeSql
    ConfigureFreeSql = (freeSql, dbConfig) =>
    {
#if (!NoTaskScheduler)
        if (dbConfig.Key == AdminDbkeys.TaskDb)
        {
            freeSql.SyncSchedulerStructure(dbConfig, ConfigureScheduler);
        }
#endif
    },

    //配置前置服务
    ConfigurePreServices = context =>
	{
        var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", context.Environment.EnvironmentName);
		if (dbConfig.Key.NotNull())
		{
			DbKeys.AppDb = dbConfig.Key;
		}
#if (MergeDb)
		AdminDbkeys.AppDb = DbKeys.AppDb;
#else
        AdminDbkeys.AppDb = "admindb";
#endif
        AdminSubscribeNames.SmsSingleSend = "app.smsSingleSend";
    },
    //配置后置服务
    ConfigurePostServices = context =>
	{
#if (!NoTaskScheduler)
        //添加任务调度，默认使用权限库作为任务调度库
        context.Services.AddTaskScheduler(AdminDbkeys.TaskDb, options =>
        {
            options.ConfigureFreeSql = ConfigureScheduler;

            //配置任务调度
            options.ConfigureFreeSchedulerBuilder = freeSchedulerBuilder =>
            {
                freeSchedulerBuilder
                .OnExecuting(task =>
                {
                    //执行任务
                })
                .UseCustomInterval(task =>
                {
                    //自定义间隔
                    var expression = CronExpression.Parse(task.IntervalArgument, CronFormat.IncludeSeconds);
                    var next = expression.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local);
                    var nextLocalTime = next?.DateTime;

                    return nextLocalTime == null ? null : nextLocalTime - DateTime.Now;
                });
            };
        });
#endif
#if (!NoCap)
        //添加cap事件总线
        var appConfig = ConfigHelper.Get<AppConfig>("appconfig", context.Environment.EnvironmentName);
		Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
			.Where(a => appConfig.AssemblyNames.Contains(a.Name))
			.Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

        //var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", context.Environment.EnvironmentName);
        //var rabbitMQ = context.Configuration.GetSection("CAP:RabbitMq").Get<RabbitMQOptions>();
        context.Services.AddCap(config =>
		{
			config.UseInMemoryStorage();
			config.UseInMemoryMessageQueue();

            //<PackageReference Include="DotNetCore.CAP.MySql" Version="8.0.0" />
            //<PackageReference Include="DotNetCore.CAP.RabbitMQ" Version="8.0.0" />

            //config.UseMySql(dbConfig.ConnectionString);
            //config.UseRabbitMQ(mqConfig => {
            //    mqConfig.HostName = rabbitMQ.HostName;
            //    mqConfig.Port = rabbitMQ.Port;
            //    mqConfig.UserName = rabbitMQ.UserName;
            //    mqConfig.Password = rabbitMQ.Password;
            //    mqConfig.ExchangeName = rabbitMQ.ExchangeName;
            //});
            config.UseDashboard();
		}).AddSubscriberAssembly(assemblies);
#endif
    },
    //配置Autofac容器
    ConfigureAutofacContainer = (builder, context) =>
    {

    },
    //配置Mvc
    ConfigureMvcBuilder = (builder, context) =>
    {
    },
    //配置后置中间件
    ConfigurePostMiddleware = context =>
	{
		var app = context.App;
		var env = app.Environment;
		var appConfig = app.Services.GetService<AppConfig>();
#if (!NoApiUI)

		#region 新版Api文档
		if (env.IsDevelopment() || appConfig.ApiUI.Enable)
		{
            app.UseApiUI(options =>
            {
                options.RoutePrefix = appConfig.ApiUI.RoutePrefix;
                var routePath = options.RoutePrefix.NotNull() ? $"{options.RoutePrefix}/" : "";
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerEndpoint($"/{routePath}swagger/{project.Code.ToLower()}/swagger.json", project.Name);
                });
            });
        }
		#endregion
#endif
	}
}).Run(args);

public partial class Program { }