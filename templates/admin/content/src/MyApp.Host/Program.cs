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
using ZhonTai.Admin.Domain;
using System;
using Cronos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using ZhonTai.Admin.Services.Msg;
using ZhonTai.Admin.Services.TaskScheduler;
using ZhonTai.Admin.Services.Msg.Events;
using Mapster;
#endif
using Autofac;
using MyApp.Api.Core.Repositories;
using System.Text;
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
        if (dbConfig.Key == AdminDbKeys.TaskDb)
        {
            freeSql.SyncSchedulerStructure(dbConfig, ConfigureScheduler);
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
            options.ConfigureFreeSql = ConfigureScheduler;

            //配置任务调度
            options.ConfigureFreeSchedulerBuilder = freeSchedulerBuilder =>
            {
                freeSchedulerBuilder
                .OnExecuting(task =>
                {
                    var taskSchedulerConfig = AppInfo.GetRequiredService<IOptions<TaskSchedulerConfig>>().Value;

                    if (task.Topic?.StartsWith("[shell]") == true)
                    {
                        var jsonArgs = JToken.Parse(task.Body);
                        var shellArgs = jsonArgs.Adapt<ShellArgs>();

                        var arguments = shellArgs.Arguments;
                        var modeulName = jsonArgs["moduleName"]?.ToString();
                        if (modeulName.NotNull())
                        {
                            //通过moduleName获取配置文件moduleName对应的Grpc远程地址
                            var grpcAddress = string.Empty;
                            if (grpcAddress.NotNull())
                            {
                                arguments = arguments.Replace("${grpcAddress}", grpcAddress, StringComparison.OrdinalIgnoreCase);
                            }
                        }

                        var fileName = shellArgs.FileName;
                        if (fileName.IsNull())
                        {
                            fileName = taskSchedulerConfig?.ProcessStartInfo?.FileName;
                        }

                        var workingDirectory = shellArgs.WorkingDirectory;
                        if (workingDirectory.IsNull())
                        {
                            workingDirectory = taskSchedulerConfig?.ProcessStartInfo?.WorkingDirectory;
                        }

                        var startInfo = new ProcessStartInfo
                        {
                            FileName = fileName,
                            Arguments = arguments,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            WorkingDirectory = workingDirectory,
                            StandardOutputEncoding = Encoding.UTF8,
                            StandardErrorEncoding = Encoding.UTF8,
                        };

                        var response = string.Empty;
                        var error = string.Empty;
                        using (var process = Process.Start(startInfo))
                        {
                            response = process.StandardOutput.ReadToEnd();
                            error = process.StandardError.ReadToEnd();

                            //if (response.NotNull())
                            //{
                            //    Console.WriteLine("Response:");
                            //    Console.WriteLine(response);
                            //}

                            //if (error.NotNull())
                            //{
                            //    Console.WriteLine("Error:");
                            //    Console.WriteLine(error);
                            //}

                            process.WaitForExit();
                        }

                        if (response.NotNull())
                            task.Remark(response);

                        if (error.NotNull())
                            throw new Exception(error);
                    }
                })
                .OnExecuted((task, taskLog) =>
                {
                    try
                    {
                        if (!taskLog.Success)
                        {
                            //发送告警邮件
                            var taskService = AppInfo.GetRequiredService<TaskService>();
                            var emailService = AppInfo.GetRequiredService<EmailService>();
                            var alerEmail = taskService.GetAlerEmailAsync(task.Id).Result;
                            var topic = task.Topic;
                            if (alerEmail.NotNull())
                            {
                                var jsonArgs = JToken.Parse(task.Body);
                                var desc = jsonArgs["desc"]?.ToString();
                                if (desc.NotNull())
                                    topic = desc;
                            }
                            alerEmail?.Split(',')?.ToList()?.ForEach(async address =>
                            {
                                await emailService.SingleSendAsync(new EmailSingleSendEvent
                                {
                                    ToEmail = new EmailSingleSendEvent.Models.EmailModel
                                    {
                                        Address = address,
                                        Name = address
                                    },
                                    Subject = "【任务调度中心】监控报警",
                                    Body = $@"<p>任务名称：{topic}</p>
<p>任务编号：{task.Id}</p>
<p>告警类型：调度失败</p>
<p>告警内容：<br/>{taskLog.Exception}</p>"
                                });
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        AppInfo.Log.Error(ex);
                    }
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
        builder.RegisterGeneric(typeof(AppRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
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