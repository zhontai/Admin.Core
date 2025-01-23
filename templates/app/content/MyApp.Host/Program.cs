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
using AdminDbKeys = ZhonTai.Admin.Core.Consts.DbKeys;
#if (!NoTaskScheduler)
using ZhonTai.Admin.Core.Db;
using System;
using ZhonTai.Admin.Services.TaskScheduler;
#endif
using Autofac;
using MyApp.Api.Core.Repositories;
#if (IsSys)
using ZhonTai.Admin.Repositories;
#endif

new HostApp(new HostAppOptions()
{
    //配置FreeSql
    ConfigureFreeSql = (freeSql, dbConfig) =>
    {
#if (!NoTaskScheduler)
        if (dbConfig.Key == AdminDbKeys.TaskDb)
        {
            freeSql.SyncSchedulerStructure(dbConfig, TaskSchedulerServiceExtensions.ConfigureScheduler);
        }
#endif
    },

    //配置前置服务
    ConfigurePreServices = context =>
	{
        var dbConfig = AppInfo.GetOptions<DbConfig>();
		if (dbConfig.Key.NotNull())
		{
			DbKeys.AppDb = dbConfig.Key;
		}
#if (MergeDb)
		AdminDbKeys.AppDb = DbKeys.AppDb;
#if (!NoTaskScheduler)
        AdminDbKeys.TaskDb = DbKeys.AppDb;
#endif
#else
        AdminDbKeys.AppDb = "admindb";
#endif
    },
    //配置后置服务
    ConfigurePostServices = context =>
	{
#if (!NoTaskScheduler)
        //添加任务调度，默认使用权限库作为任务调度库
        context.Services.AddTaskScheduler(AdminDbKeys.TaskDb, options =>
        {
            options.ConfigureFreeSql = TaskSchedulerServiceExtensions.ConfigureScheduler;

            //配置任务调度
            options.ConfigureFreeSchedulerBuilder = freeSchedulerBuilder =>
            {
                static void OnExecuting(TaskInfo task)
                {
                    if (task.Topic?.StartsWith("[shell]") == true)
                    {
                        TaskSchedulerServiceExtensions.ExecuteGrpc(task);
                    }
                }

                freeSchedulerBuilder
                .OnExecuting(task => OnExecuting(task))
                .OnExecuted((task, taskLog) =>
                {
                    try
                    {
                        if (!taskLog.Success)
                        {
                            var taskService = AppInfo.GetRequiredService<TaskService>();
                            var taskInfo = taskService.GetAsync(task.Id).Result;

                            //失败重试
                            TaskSchedulerServiceExtensions.FailedRetry(taskInfo, task, taskLog, OnExecuting);

                            //发送告警邮件
                            TaskSchedulerServiceExtensions.SendAlarmEmail(taskInfo, task, taskLog);
                        }
                    }
                    catch (Exception ex)
                    {
                        AppInfo.Log.Error(ex);
                    }
                });
            };
        });
#endif
#if (!NoCap)
        //添加cap事件总线
        var appConfig = AppInfo.GetOptions<AppConfig>();
		Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
			.Where(a => appConfig.AssemblyNames.Contains(a.Name))
			.Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

        //var dbConfig = AppInfo.GetOptions<DbConfig>();
        //var rabbitMQ = context.Configuration.GetSection("CAP:RabbitMq").Get<RabbitMQOptions>();
        context.Services.AddCap(config =>
		{
			config.UseInMemoryStorage();
			config.UseInMemoryMessageQueue();

            //<PackageReference Include="DotNetCore.CAP.MySql" Version="8.3.2" />
            //<PackageReference Include="DotNetCore.CAP.RabbitMQ" Version="8.3.2" />

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
        builder.RegisterGeneric(typeof(AppRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
#if (IsSys)
        builder.RegisterGeneric(typeof(AdminRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
#endif
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
		var appConfig = AppInfo.GetOptions<AppConfig>();
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