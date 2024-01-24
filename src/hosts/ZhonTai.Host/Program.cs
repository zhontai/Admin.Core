using Cronos;
using FreeScheduler;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Savorboard.CAP.InMemoryMessageQueue;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ZhonTai;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Startup;
using ZhonTai.Admin.Domain;
using ZhonTai.Admin.Services.Msg;
using ZhonTai.Admin.Services.Msg.Events;
using ZhonTai.Admin.Services.TaskScheduler;
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
                    switch (task.Topic)
                    {
                        //执行shell
                        case "[system]shell":
                            var jsonArgs = JToken.Parse(task.Body);
                            var shellArgs = jsonArgs.Adapt<ShellArgs>();

                            var startInfo = new ProcessStartInfo
                            {
                                FileName = shellArgs.FileName,
                                Arguments = shellArgs.Arguments,
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                WorkingDirectory = shellArgs.WorkingDirectory
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

                            break;
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
                                await emailService.SingleSendAsync(new EamilSingleSendEvent
                                {
                                    ToEmail = new EamilSingleSendEvent.Models.EmailModel
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
                var routePath = options.RoutePrefix.NotNull() ? $"{ options.RoutePrefix}/" : "";
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerEndpoint($"/{routePath}swagger/{project.Code.ToLower()}/swagger.json", project.Name);
                });
            });
		}
        #endregion
	},

    ConfigureSwaggerUI = options =>
    {
        //options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.Full);
    }
}).Run(args);

#if DEBUG
public partial class Program { }
#endif