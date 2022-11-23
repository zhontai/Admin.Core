using FreeScheduler;
using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Admin.Tools.TaskScheduler;

public static class TaskSchedulerServiceExtensions
{
    public static IServiceProvider ServiceProvider { get; private set; }

    /// <summary>
    /// 添加任务调度
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureOptions"></param>
    public static IServiceCollection AddTaskScheduler(this IServiceCollection services, Action<TaskSchedulerOptions> configureOptions = null)
    {
        return services.AddTaskScheduler(DbKeys.AppDb, configureOptions);
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
            FreeSql = ServiceProvider.GetService<FreeSqlCloud>().Use(dbKey)
        };
        configureOptions?.Invoke(options);

        var freeSql = options.FreeSql;

        freeSql.CodeFirst
        .ConfigEntity<TaskInfo>(a =>
        {
            a.Name("ad_task");
            a.Property(b => b.Id).IsPrimary(true);
            a.Property(b => b.Body).StringLength(-1);
            a.Property(b => b.Interval).MapType(typeof(int));
            a.Property(b => b.IntervalArgument).StringLength(1024);
            a.Property(b => b.Status).MapType(typeof(int));
            a.Property(b => b.CreateTime).ServerTime(DateTimeKind.Local);
            a.Property(b => b.LastRunTime).ServerTime(DateTimeKind.Local);
        })
        .ConfigEntity<TaskLog>(a =>
        {
            a.Name("ad_task_log");
            a.Property(b => b.Exception).StringLength(-1);
            a.Property(b => b.Remark).StringLength(-1);
            a.Property(b => b.CreateTime).ServerTime(DateTimeKind.Local);
        });

        options.ConfigureFreeSql?.Invoke(freeSql);

        var dbConfig = ServiceProvider.GetService<DbConfig>();
        if (dbConfig.SyncStructure)
        {
            freeSql.CodeFirst.SyncStructure<TaskInfo>();
            freeSql.CodeFirst.SyncStructure<TaskLog>();
        }

        if (options.TaskHandler != null && options.CustomTaskHandler == null)
        {
            //开启任务
            var scheduler = new Scheduler(options.TaskHandler);
            services.AddSingleton(scheduler);
        }
        else if (options.TaskHandler != null && options.CustomTaskHandler != null)
        {
            //开启自定义任务
            var scheduler = new Scheduler(options.TaskHandler, options.CustomTaskHandler);
            services.AddSingleton(scheduler);
        }

        return services;
    }

    /// <summary>
    /// 使用任务调度
    /// </summary>
    /// <param name="app"></param>
    public static void UseTaskScheduler(this IApplicationBuilder app)
    {
        ServiceProvider = app.ApplicationServices;
    }
}
