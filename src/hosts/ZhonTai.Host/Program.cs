using COSXML.Callback;
using Cronos;
using FreeScheduler;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Savorboard.CAP.InMemoryMessageQueue;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

    ConfigurePreServices = context =>
    {
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
                void OnExecuting(TaskInfo task)
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
                            if (taskInfo != null && taskInfo.FailRetryCount > 0)
                            {
                                var retryRound = 0;
                                var failRetryCount = taskInfo.FailRetryCount;
                                var failRetryInterval = taskInfo.FailRetryInterval > 0 ? taskInfo.FailRetryInterval.Value : 10;
                                var scheduler = AppInfo.GetRequiredService<Scheduler>();
                                var currentRound = taskLog.Round;
                                void OnFailedCallBak()
                                {
                                    failRetryCount--;
                                    retryRound++;
                                    var startdt = DateTime.UtcNow;
                                    var result = new TaskLog
                                    {
                                        CreateTime = DateTime.UtcNow.Add(scheduler.TimeOffset),
                                        TaskId = task.Id,
                                        Round = currentRound,
                                        Remark = $"第{retryRound}次失败重试",
                                        Success = true
                                    };

                                    try
                                    {
                                        OnExecuting(task);
                                    }
                                    catch (Exception ex)
                                    {
                                        result.Success = false;
                                        result.Exception = ex.InnerException == null ? $"{ex.Message}\r\n{ex.StackTrace}" : $"{ex.Message}\r\n{ex.StackTrace}\r\n\r\nInnerException: {ex.InnerException.Message}\r\n{ex.InnerException.StackTrace}";

                                        if (failRetryCount > 0)
                                        {
                                            scheduler.AddTempTask(TimeSpan.FromSeconds(failRetryInterval), OnFailedCallBak);
                                        }
                                    }
                                    finally
                                    {
                                        result.ElapsedMilliseconds = (long)DateTime.UtcNow.Subtract(startdt).TotalMilliseconds;
                                        var taskLogService = AppInfo.GetRequiredService<TaskLogService>();
                                        taskLogService.Add(result);
                                    }
                                }

                                scheduler.AddTempTask(TimeSpan.FromSeconds(failRetryInterval), OnFailedCallBak);
                            }

                            //发送告警邮件
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
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerEndpoint($"/{options.RoutePrefix}/swagger/{project.Code.ToLower()}/swagger.json", project.Name);
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
public partial class Program { }
#endif