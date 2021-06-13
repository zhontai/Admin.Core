using Admin.Core.Common.Auth;
using Admin.Core.Common.BaseModel;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Dbs;
using Admin.Core.Common.Helpers;
using Admin.Core.Model.Admin;
using Admin.Core.Repository;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Admin.Core.Db
{
    public static class DBServiceCollectionExtensions
    {
        /// <summary>
        /// 添加数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        public async static Task AddDbAsync(this IServiceCollection services, IHostEnvironment env)
        {
            services.AddScoped<MyUnitOfWorkManager>();

            var dbConfig = new ConfigHelper().Get<DbConfig>("dbconfig", env.EnvironmentName);

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

            //配置实体
            var appConfig = new ConfigHelper().Get<AppConfig>("appconfig", env.EnvironmentName);
            DbHelper.ConfigEntity(fsql, appConfig);

            #region 初始化数据库

            //同步结构
            if (dbConfig.SyncStructure)
            {
                DbHelper.SyncStructure(fsql, dbConfig: dbConfig, appConfig: appConfig);
            }

            var user = services.BuildServiceProvider().GetService<IUser>();

            #region 审计数据

            //计算服务器时间
            var serverTime = fsql.Select<DualEntity>().Limit(1).First(a => DateTime.UtcNow);
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

            //生成数据包
            if (dbConfig.GenerateData && !dbConfig.CreateDb && !dbConfig.SyncData)
            {
                await DbHelper.GenerateSimpleJsonDataAsync(fsql, appConfig);
            }

            #region 监听Curd操作

            if (dbConfig.Curd)
            {
                fsql.Aop.CurdBefore += (s, e) =>
                {
                    Console.WriteLine($"{e.Sql}\r\n");
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
}