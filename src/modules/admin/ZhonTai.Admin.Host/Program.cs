using Autofac;
using FreeScheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using Savorboard.CAP.InMemoryMessageQueue;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Startup;
using ZhonTai.Admin.Services.TaskScheduler;
using ZhonTai.Admin.Tools.TaskScheduler;
using ZhonTai.ApiUI;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Repositories;
using ZhonTai.Admin.Core.Extensions;
using DotNetCore.CAP.Messages;
using System.Text.Encodings.Web;
using System.Text.Json;

new HostApp(new HostAppOptions
{
    //前置配置FreeSql
    ConfigurePreFreeSql = (freeSql, dbConfig) =>
    {
        freeSql.UseJsonMap(); //启用JsonMap功能
        freeSql.UseLogDb(dbConfig); //使用日志数据库
    },
    //配置FreeSql构建器
    ConfigureFreeSqlBuilder = (freeSqlBuilder, dbConfig) =>
    {
        if (dbConfig.Type == FreeSql.DataType.QuestDb)
        {
            freeSqlBuilder.UseQuestDbRestAPI("http://localhost:9000", "admin", "quest");
        }
    },
    //配置FreeSql
    ConfigureFreeSql = (freeSql, dbConfig) =>
    {
        if (dbConfig.Key == DbKeys.TaskDb)
        {
            freeSql.SyncSchedulerStructure(dbConfig, TaskSchedulerServiceExtensions.ConfigureScheduler);
        }
    },

    //配置前置服务
    ConfigurePreServices = context =>
    {
        DbKeys.LogDb = "logdb";

        context.Services.Configure<TaskSchedulerConfig>(context.Configuration.GetSection("TaskScheduler"));
    },
    //配置后置服务
    ConfigurePostServices = context =>
    {
        //context.Services.AddTiDb(context);

        //添加cap事件总线
        var appConfig = AppInfo.GetRequiredService<AppConfig>(false);
        Assembly[] assemblies = AssemblyHelper.GetAssemblyList(appConfig.AssemblyNames);

        //var dbConfig = AppInfo.GetRequiredService<DbConfig>(false);
        //var rabbitMQ = context.Configuration.GetSection("CAP:RabbitMq").Get<RabbitMQOptions>();
        context.Services.AddCap(config =>
        {
            config.DefaultGroupName = "zhontai.admin";
            //开发阶段不同开发人员的消息区分，可以通过配置版本号实现
            config.Version = "v1";

            config.UseInMemoryStorage();
            config.UseInMemoryMessageQueue();
            config.FailedRetryCount = 5;
            config.FailedRetryInterval = 15;
            config.EnablePublishParallelSend = true;
            config.UseStorageLock = true;
            config.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            //config.UseMySql(dbConfig.ConnectionString);
            //config.UseRabbitMQ(mqConfig => {
            //    mqConfig.HostName = rabbitMQ.HostName;
            //    mqConfig.Port = rabbitMQ.Port;
            //    mqConfig.UserName = rabbitMQ.UserName;
            //    mqConfig.Password = rabbitMQ.Password;
            //    mqConfig.ExchangeName = rabbitMQ.ExchangeName;
            //});

            config.FailedThresholdCallback = failed =>
            {
                AppInfo.Log.Error($@"消息处理失败！类型: {failed.MessageType}, 
已重试 {config.FailedRetryCount} 次仍失败，需人工处理。消息名称: {failed.Message.GetName()}");
            };

            config.UseDashboard();
        }).AddSubscriberAssembly(assemblies);

        //添加任务调度
        context.Services.AddTaskScheduler(DbKeys.TaskDb, options =>
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
    },

    //配置Autofac容器
    ConfigureAutofacContainer = (builder, context) => 
    {
        builder.RegisterGeneric(typeof(AdminRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
        builder.RegisterGeneric(typeof(LogRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
    },

    //配置Mvc
    ConfigureMvcBuilder = (builder, context) => { },

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
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerEndpoint($"/{options.RoutePrefix}/swagger/{project.Code.ToLower()}/swagger.json",
                        project.Name);
                });
            });
        }

        #endregion
    },

    ConfigureSwaggerUI = options =>
    {
        //options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.Full);
    }
}).Run(args, typeof(Program).Assembly);

#if DEBUG
public partial class Program
{
}
#endif