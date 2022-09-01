using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using StackExchange.Profiling;
using FreeSql;
using FreeSql.Internal.CommonProvider;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Admin.Core.Dbs;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Core.Startup;

namespace ZhonTai.Admin.Core.Db;

public static class DBServiceCollectionExtensions
{
    /// <summary>
    /// 添加数据库
    /// </summary>
    /// <param name="services"></param>
    /// <param name="env"></param>
    /// <param name="hostAppOptions"></param>
    /// <returns></returns>
    public async static Task AddDbAsync(this IServiceCollection services, IHostEnvironment env, HostAppOptions hostAppOptions)
    {
        services.AddScoped<DbUnitOfWorkManager>();

        var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", env.EnvironmentName);

        //创建数据库
        if (dbConfig.CreateDb)
        {
            await DbHelper.CreateDatabaseAsync(dbConfig);
        }

        #region FreeSql

        var freeSqlBuilder = new FreeSqlBuilder()
                .UseConnectionString(dbConfig.Type, dbConfig.ConnectionString)
                .UseAutoSyncStructure(false)
                .UseLazyLoading(false)
                .UseNoneCommandParameter(true);

        hostAppOptions?.ConfigureFreeSqlBuilder?.Invoke(freeSqlBuilder);

        #region 监听所有命令

        if (dbConfig.MonitorCommand)
        {
            freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
            {
                //Console.WriteLine($"{cmd.CommandText}\n{traceLog}\r\n");
                Console.WriteLine($"{cmd.CommandText}\r\n");
            });
        }

        #endregion 监听所有命令

        var fsql = freeSqlBuilder.Build();
        fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDeleted == false);

        //配置实体
        var appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName);
        DbHelper.ConfigEntity(fsql, appConfig);

        hostAppOptions?.ConfigureFreeSql?.Invoke(fsql);
        #region 初始化数据库

        //同步结构
        if (dbConfig.SyncStructure)
        {
            DbHelper.SyncStructure(fsql, dbConfig: dbConfig, appConfig: appConfig);
        }

        var user = services.BuildServiceProvider().GetService<IUser>();

        #region 审计数据

        //计算服务器时间
        var selectProvider = fsql.Select<object>() as Select0Provider;
        var serverTime = fsql.Select<object>().WithSql($"select {selectProvider._commonUtils.NowUtc} a").First(a => Convert.ToDateTime("a"));
        var timeOffset = DateTime.UtcNow.Subtract(serverTime);
        DbHelper.TimeOffset = timeOffset;
        fsql.Aop.AuditValue += (s, e) =>
        {
            DbHelper.AuditValue(e, timeOffset, user);
        };

        #endregion 审计数据

        //同步数据
        if (dbConfig.SyncData)
        {
            await DbHelper.SyncDataAsync(fsql, dbConfig, appConfig);
        }

        #endregion 初始化数据库

        //生成数据
        if (dbConfig.GenerateData && !dbConfig.CreateDb && !dbConfig.SyncData)
        {
            await DbHelper.GenerateDataAsync(fsql, appConfig);
        }

        #region 监听Curd操作

        if (dbConfig.Curd)
        {
            fsql.Aop.CurdBefore += (s, e) =>
            {
                if (appConfig.MiniProfiler)
                {
                    MiniProfiler.Current.CustomTiming("CurdBefore", e.Sql);
                }
                Console.WriteLine($"{e.Sql}\r\n");
            };
            fsql.Aop.CurdAfter += (s, e) =>
            {
                if (appConfig.MiniProfiler)
                {
                    MiniProfiler.Current.CustomTiming("CurdAfter", $"{e.ElapsedMilliseconds}");
                }
            };
        }

        #endregion 监听Curd操作

        if (appConfig.Tenant)
        {
            fsql.GlobalFilter.Apply<ITenant>("Tenant", a => a.TenantId == user.TenantId);
        }

        #endregion FreeSql

        services.AddSingleton(fsql);

        //导入多数据库
        if (null != dbConfig.Dbs)
        {
            foreach (var multiDb in dbConfig.Dbs)
            {
                switch (multiDb.Name)
                {
                    case nameof(MySqlDb):
                        var mdb = CreateMultiDbBuilder(multiDb).Build<MySqlDb>();
                        services.AddSingleton(mdb);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 创建多数据库构建器
    /// </summary>
    /// <param name="multiDb"></param>
    /// <returns></returns>
    private static FreeSqlBuilder CreateMultiDbBuilder(MultiDb multiDb)
    {
        return new FreeSqlBuilder()
        .UseConnectionString(multiDb.Type, multiDb.ConnectionString)
        .UseAutoSyncStructure(false)
        .UseLazyLoading(false)
        .UseNoneCommandParameter(true);
    }
}