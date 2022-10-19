using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using StackExchange.Profiling;
using FreeSql;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Core.Startup;
using ZhonTai.Admin.Core.Consts;
using System.Linq;
using System.Collections.Concurrent;
using System.Reflection;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Core.Db.Transaction;

namespace ZhonTai.Admin.Core.Db;

public static class DBServiceCollectionExtensions
{
    /// <summary>
    /// 注册数据库
    /// </summary>
    /// <param name="freeSqlCloud"></param>
    /// <param name="user"></param>
    /// <param name="dbConfig"></param>
    /// <param name="appConfig"></param>
    /// <param name="hostAppOptions"></param>
    private static void RegisterDb(
        FreeSqlCloud freeSqlCloud,
        IUser user,
        DbConfig dbConfig,
        AppConfig appConfig,
        HostAppOptions hostAppOptions
    )
    {
        //注册数据库
        freeSqlCloud.Register(dbConfig.Key, () =>
        {
            //创建数据库
            if (dbConfig.CreateDb)
            {
                DbHelper.CreateDatabaseAsync(dbConfig).Wait();
            }

            var providerType = dbConfig.ProviderType.NotNull() ? Type.GetType(dbConfig.ProviderType) : null;
            var freeSqlBuilder = new FreeSqlBuilder()
                    .UseConnectionString(dbConfig.Type, dbConfig.ConnectionString, providerType)
                    .UseAutoSyncStructure(false)
                    .UseLazyLoading(false)
                    .UseNoneCommandParameter(true);

            if (dbConfig.SlaveList?.Length > 0)
            {
                var slaveList = dbConfig.SlaveList.Select(a => a.ConnectionString).ToArray();
                var slaveWeightList = dbConfig.SlaveList.Select(a => a.Weight).ToArray();
                freeSqlBuilder.UseSlave(slaveList).UseSlaveWeight(slaveWeightList);
            }

            hostAppOptions?.ConfigureFreeSqlBuilder?.Invoke(freeSqlBuilder);

            #region 监听所有命令

            if (dbConfig.MonitorCommand)
            {
                freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
                {
                    //Console.WriteLine($"{cmd.CommandText}\n{traceLog}{Environment.NewLine}");
                    Console.WriteLine($"{cmd.CommandText}{Environment.NewLine}");
                });
            }

            #endregion 监听所有命令

            var fsql = freeSqlBuilder.Build();

            //软删除过滤器
            fsql.GlobalFilter.ApplyOnly<IDelete>(FilterNames.Delete, a => a.IsDeleted == false);

            //租户过滤器
            if (appConfig.Tenant)
            {
                fsql.GlobalFilter.ApplyOnlyIf<ITenant>(FilterNames.Tenant, () => user?.Id > 0, a => a.TenantId == user.TenantId);
            }

            //数据权限过滤器
            fsql.GlobalFilter.ApplyOnlyIf<IData>(FilterNames.Self,
                () =>
                {
                    if (!(user?.Id > 0))
                        return false;
                    var dataPermission = user.DataPermission;
                    if (user.Type == UserType.DefaultUser && dataPermission != null)
                        return dataPermission.DataScope != DataScope.All && dataPermission.OrgIds.Count == 0;
                    return false;
                },
                a => a.OwnerId == user.Id
            );
            fsql.GlobalFilter.ApplyOnlyIf<IData>(FilterNames.Data,
                () =>
                {
                    if (!(user?.Id > 0))
                        return false;
                    var dataPermission = user.DataPermission;
                    if (user.Type == UserType.DefaultUser && dataPermission != null)
                        return dataPermission.DataScope != DataScope.All && dataPermission.OrgIds.Count > 0;
                    return false;
                },
                a => a.OwnerId == user.Id || user.DataPermission.OrgIds.Contains(a.OwnerOrgId.Value)
            );

            //配置实体
            DbHelper.ConfigEntity(fsql, appConfig, dbConfig);

            hostAppOptions?.ConfigureFreeSql?.Invoke(fsql);

            #region 初始化数据库

            //同步结构
            if (dbConfig.SyncStructure)
            {
                DbHelper.SyncStructure(fsql, dbConfig: dbConfig, appConfig: appConfig);
            }

            #region 审计数据

            //计算服务器时间
            var serverTime = fsql.Ado.QuerySingle(() => DateTime.UtcNow);
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
                DbHelper.SyncDataAsync(fsql, dbConfig, appConfig).Wait();
            }

            #endregion 初始化数据库

            //生成数据
            if (dbConfig.GenerateData && !dbConfig.CreateDb && !dbConfig.SyncData)
            {
                DbHelper.GenerateDataAsync(fsql, appConfig, dbConfig).Wait();
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
                    Console.WriteLine($"{e.Sql}{Environment.NewLine}");
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

            return fsql;
        });

        //执行注册数据库
        var fsql = freeSqlCloud.Use(dbConfig.Key);
        if (dbConfig.SyncStructure)
        {
            var _ = fsql.CodeFirst;
        }
    }

    /// <summary>
    /// 添加数据库
    /// </summary>
    /// <param name="services"></param>
    /// <param name="env"></param>
    /// <param name="hostAppOptions"></param>
    /// <returns></returns>
    public static void AddDb(this IServiceCollection services, IHostEnvironment env, HostAppOptions hostAppOptions)
    {
        var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", env.EnvironmentName);
        var appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName);
        var user = services.BuildServiceProvider().GetService<IUser>();
        var freeSqlCloud = appConfig.DistributeKey.IsNull() ? new FreeSqlCloud() : new FreeSqlCloud(appConfig.DistributeKey);
        if (dbConfig.Key.NotNull())
        {
            DbKeys.MasterDb = dbConfig.Key;
        }
        RegisterDb(freeSqlCloud, user, dbConfig, appConfig, hostAppOptions);

        //注册多数据库
        if (dbConfig.Dbs?.Length > 0)
        {
            foreach (var db in dbConfig.Dbs)
            {
                RegisterDb(freeSqlCloud, user, db, appConfig, null);
            }
        }

        services.AddSingleton<IFreeSql>(freeSqlCloud);
        services.AddSingleton(freeSqlCloud);
        services.AddScoped<UnitOfWorkManagerCloud>();
    }

    /// <summary>
    /// 添加TiDb数据库
    /// </summary>
    /// <param name="_"></param>
    /// <param name="context"></param>
    /// <param name="version">版本</param>
    public static void AddTiDb(this IServiceCollection _, HostAppContext context, string version = "8.0")
    {
        var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", context.Environment.EnvironmentName);
        var _dicMySqlVersion = typeof(FreeSqlGlobalExtensions).GetField("_dicMySqlVersion", BindingFlags.NonPublic | BindingFlags.Static);
        var dicMySqlVersion = new ConcurrentDictionary<string, string>();
        dicMySqlVersion[dbConfig.ConnectionString] = version;
        _dicMySqlVersion.SetValue(new ConcurrentDictionary<string, string>(), dicMySqlVersion);
    }
}