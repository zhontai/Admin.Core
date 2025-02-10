using Cronos;
using FreeScheduler;
using FreeSql;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Domain;
using ZhonTai.Admin.Services.Email;
using ZhonTai.Admin.Services.Email.Events;
using ZhonTai.Admin.Services.TaskScheduler;
using ZhonTai.Admin.Services.TaskScheduler.Dto;

namespace ZhonTai.Admin.Tools.TaskScheduler;

public static class TaskSchedulerServiceExtensions
{
    public static void ConfigureScheduler(IFreeSql fsql)
    {
        fsql.CodeFirst
        .ConfigEntity<TaskInfo>(a =>
        {
            a.Name("base_task").OldName("app_task");
        })
        .ConfigEntity<TaskLog>(a =>
        {
            a.Name("base_task_log").OldName("app_task_log");
        })
        .ConfigEntity<TaskInfoExt>(a =>
        {
            a.Name("base_task_ext").OldName("app_task_ext");
        });
    }

    public static IServiceProvider ServiceProvider { get; private set; }

    /// <summary>
    /// 添加任务调度
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureOptions"></param>
    public static IServiceCollection AddTaskScheduler(this IServiceCollection services, Action<TaskSchedulerOptions> configureOptions = null)
    {
        return services.AddTaskScheduler(DbKeys.TaskDb, configureOptions);
    }

    /// <summary>
    /// 执行Grpc远程调用
    /// </summary>
    /// <param name="task"></param>
    /// <param name="grpcAddress"></param>
    /// <exception cref="Exception"></exception>
    public static void ExecuteGrpc(TaskInfo task, string grpcAddress = "")
    {
        var taskSchedulerConfig = AppInfo.GetRequiredService<IOptions<TaskSchedulerConfig>>().Value;
        var jsonArgs = JToken.Parse(task.Body);
        var shellArgs = jsonArgs.Adapt<ShellArgs>();

        var arguments = shellArgs.Arguments;
        var modeulName = jsonArgs["moduleName"]?.ToString();
        if (modeulName.NotNull())
        {
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

    /// <summary>
    /// 失败重试
    /// </summary>
    /// <param name="taskInfo"></param>
    /// <param name="task"></param>
    /// <param name="taskLog"></param>
    /// <param name="onExecuting"></param>
    public static void FailedRetry(TaskGetOutput taskInfo, TaskInfo task, TaskLog taskLog, Action<TaskInfo> onExecuting)
    {
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
                    onExecuting(task);
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
    }

    /// <summary>
    /// 发送告警邮件
    /// </summary>
    /// <param name="taskInfo"></param>
    /// <param name="task"></param>
    /// <param name="taskLog"></param>
    public static void SendAlarmEmail(TaskGetOutput taskInfo, TaskInfo task, TaskLog taskLog)
    {
        var alarmEmail = taskInfo?.AlarmEmail;
        var taskSchedulerConfig = AppInfo.GetRequiredService<IOptionsMonitor<TaskSchedulerConfig>>().CurrentValue;
        if (taskSchedulerConfig.AlerEmail != null && taskSchedulerConfig.AlerEmail.Enable)
        {
            var emailService = AppInfo.GetRequiredService<EmailService>();
            if (alarmEmail.IsNull())
            {
                alarmEmail = taskSchedulerConfig.AlerEmail.Address;
            }
            var topic = task.Topic;
            if (alarmEmail.NotNull())
            {
                var jsonArgs = JToken.Parse(task.Body);
                var desc = jsonArgs["desc"]?.ToString();
                if (desc.NotNull())
                    topic = desc;
            }
            alarmEmail?.Split(',')?.ToList()?.ForEach(async address =>
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

    /// <summary>
    /// 添加任务调度
    /// </summary>
    /// <param name="services"></param>
    /// <param name="dbKey"></param>
    /// <param name="configureOptions"></param>
    public static IServiceCollection AddTaskScheduler(this IServiceCollection services, string dbKey, Action<TaskSchedulerOptions> configureOptions = null)
    {
        ServiceProvider = services.BuildServiceProvider();
        var options = new TaskSchedulerOptions()
        {
            FreeSqlCloud = ServiceProvider.GetService<FreeSqlCloud>(),
            FreeSql = ServiceProvider.GetService<FreeSqlCloud>().Use(dbKey)
        };
        configureOptions?.Invoke(options);

        var freeSql = options.FreeSql;

        var dbConfig = AppInfo.GetOptions<DbConfig>();
        freeSql.SyncSchedulerStructure(dbConfig, options.ConfigureFreeSql);

        var freeSchedulerBuilder = new FreeSchedulerBuilder()
        .UseStorage(freeSql)
        .UseTimeZone(TimeSpan.FromHours(8))
        .UseCustomInterval(task =>
        {
            var expression = CronExpression.Parse(task.IntervalArgument, CronFormat.IncludeSeconds);
            var next = expression.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local);
            var nextLocalTime = next?.DateTime;

            return nextLocalTime == null ? null : nextLocalTime - DateTime.Now;
        });

        options.ConfigureFreeSql = ConfigureScheduler;

        options.ConfigureFreeSchedulerBuilder?.Invoke(freeSchedulerBuilder);

        var scheduler = freeSchedulerBuilder.Build();

        services.AddSingleton(scheduler);

        return services;
    }
}
