using FreeScheduler;
using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db;

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
            FreeSqlCloud = ServiceProvider.GetService<FreeSqlCloud>(),
            FreeSql = ServiceProvider.GetService<FreeSqlCloud>().Use(dbKey)
        };
        configureOptions?.Invoke(options);

        var freeSql = options.FreeSql;

        var dbConfig = ServiceProvider.GetService<DbConfig>();
        freeSql.SyncSchedulerStructure(dbConfig, options.ConfigureFreeSql);

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
