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
using MyApp.Api.Core.Handlers;
using FreeScheduler;
#endif
using AdminDbkeys = ZhonTai.Admin.Core.Consts.DbKeys;
using AdminSubscribeNames = ZhonTai.Admin.Core.Consts.SubscribeNames;
using ZhonTai.Admin.Core.Db;

new HostApp(new HostAppOptions()
{
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
		context.Services.AddTaskScheduler(DbKeys.AppDb, options =>

        {
			options.ConfigureFreeSql = freeSql =>
			{
				freeSql.CodeFirst
				//配置任务表
				.ConfigEntity<TaskInfo>(a =>
				{
					a.Name("app_task");
				})
				//配置任务日志表
				.ConfigEntity<TaskLog>(a =>
				{
					a.Name("app_task_log");
				});
			};

			//模块任务处理器
			options.TaskHandler = new AppTaskHandler(options.FreeSqlCloud, DbKeys.AppDb);
			//模块自定义任务处理器，解析cron表达式
			options.CustomTaskHandler = new AppCustomTaskHandler();
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

			//<PackageReference Include="DotNetCore.CAP.MySql" Version="7.1.0" />
			//<PackageReference Include="DotNetCore.CAP.RabbitMQ" Version="7.1.0" />

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
    //配置FreeSql
    ConfigureFreeSql = (freeSql, dbConfig) =>
    {
#if (!NoTaskScheduler)
        if (dbConfig.Key == DbKeys.AppDb)
        {
            freeSql.SyncSchedulerStructure(dbConfig, (fsql) =>
            {
                fsql.CodeFirst
                .ConfigEntity<TaskInfo>(a =>
                {
                    a.Name("app_task");
                })
                .ConfigEntity<TaskLog>(a =>
                {
                    a.Name("app_task_log");
                });
            });
        }
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
#if (!NoTaskScheduler)

		//使用任务调度
		app.UseTaskScheduler();
#endif
	}
}).Run(args);

public partial class Program { }