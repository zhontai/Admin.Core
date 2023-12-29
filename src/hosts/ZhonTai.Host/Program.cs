using Cronos;
using FreeScheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Savorboard.CAP.InMemoryMessageQueue;
using System;
using System.Reflection;
using ZhonTai;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Startup;
using ZhonTai.Admin.Domain;
using ZhonTai.Admin.Tools.TaskScheduler;
using ZhonTai.ApiUI;
using ZhonTai.Common.Helpers;

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

new HostApp(new HostAppOptions
{
    //配置FreeSql
    ConfigureFreeSql = (freeSql, dbConfig) =>
    {
        if (dbConfig.Key == DbKeys.TaskDb)
        {
            freeSql.SyncSchedulerStructure(dbConfig, ConfigureScheduler);
        }
    },

	//配置后置服务
	ConfigurePostServices = context =>
	{
        //context.Services.AddTiDb(context);

        //添加cap事件总线
        var appConfig = ConfigHelper.Get<AppConfig>("appconfig", context.Environment.EnvironmentName);
        Assembly[] assemblies = AssemblyHelper.GetAssemblyList(appConfig.AssemblyNames);

        //var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", context.Environment.EnvironmentName);
        //var rabbitMQ = context.Configuration.GetSection("CAP:RabbitMq").Get<RabbitMQOptions>();
        context.Services.AddCap(config =>
        {
            config.UseInMemoryStorage();
            config.UseInMemoryMessageQueue();

            //<PackageReference Include="DotNetCore.CAP.MySql" Version="7.1.1" />
            //<PackageReference Include="DotNetCore.CAP.RabbitMQ" Version="7.1.1" />

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

        //添加任务调度
        context.Services.AddTaskScheduler(DbKeys.TaskDb, options =>
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
	}
}).Run(args);

#if DEBUG
public partial class Program { }
#endif