using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System;
using ZhonTai.Admin.Core;
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
        return services.AddTaskScheduler(DbKeys.TaskDb, configureOptions);
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
        .UseTimeZone(TimeSpan.FromHours(8));

        options.ConfigureFreeSchedulerBuilder?.Invoke(freeSchedulerBuilder);

        var scheduler = freeSchedulerBuilder.Build();

        services.AddSingleton(scheduler);

        return services;
    }
}
