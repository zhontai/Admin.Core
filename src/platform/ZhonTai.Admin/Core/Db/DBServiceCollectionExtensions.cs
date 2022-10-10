using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using StackExchange.Profiling;
using FreeSql;
using FreeSql.Internal.CommonProvider;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Core.Startup;
using ZhonTai.Admin.Core.Consts;
using System.Linq;
using System.Collections.Concurrent;
using System.Reflection;

namespace ZhonTai.Admin.Core.Db;

public static class DBServiceCollectionExtensions
{
    /// <summary>
    /// 添加主数据库
    /// </summary>
    /// <param name="services"></param>
    /// <param name="freeSqlCloud"></param>
    /// <param name="env"></param>
    /// <param name="hostAppOptions"></param>
    /// <returns></returns>
    public static void AddAdminDb(this IServiceCollection services, FreeSqlCloud freeSqlCloud, IHostEnvironment env, HostAppOptions hostAppOptions)
    {
        var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", env.EnvironmentName);
        var appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName);

        //注册主库
        freeSqlCloud.Register(DbKeys.MasterDbKey, () =>
        {
            //创建数据库
            if (dbConfig.CreateDb)
            {
                DbHelper.CreateDatabaseAsync(dbConfig).Wait();
            }

            #region FreeSql

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
                    //Console.WriteLine($"{cmd.CommandText}\n{traceLog}\r\n");
                    Console.WriteLine($"{cmd.CommandText}\r\n");
                });
            }

            #endregion 监听所有命令

            var fsql = freeSqlBuilder.Build();
            fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDeleted == false);

            //配置实体
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
                DbHelper.SyncDataAsync(fsql, dbConfig, appConfig).Wait();
            }

            #endregion 初始化数据库

            //生成数据
            if (dbConfig.GenerateData && !dbConfig.CreateDb && !dbConfig.SyncData)
            {
                DbHelper.GenerateDataAsync(fsql, appConfig).Wait();
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

            return fsql;
        });

        //注册多数据库
        if (dbConfig.Dbs?.Length > 0)
        {
            foreach (var db in dbConfig.Dbs)
            {
                freeSqlCloud.Register(DbKeys.MultiDbKey + db.Key, () =>
                {
                    #region FreeSql

                    var freeSqlBuilder = new FreeSqlBuilder()
                            .UseConnectionString(db.Type, db.ConnectionString, db.ProviderType.NotNull() ? Type.GetType(db.ProviderType) : null)
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

                    #endregion FreeSql

                    return fsql;
                });
            }
        }
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