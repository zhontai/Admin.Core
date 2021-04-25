using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FreeSql;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Dbs;
using Admin.Core.Model.Admin;
using System.Reflection;
using Admin.Core.Common.Attributes;

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
            #endregion

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

            //同步数据
            if (dbConfig.SyncData)
            {
                await DbHelper.SyncDataAsync(fsql, dbConfig);
            }
            #endregion

            //生成数据包
            if (dbConfig.GenerateData && !dbConfig.CreateDb && !dbConfig.SyncData)
            {
                await DbHelper.GenerateSimpleJsonDataAsync(fsql);
            }

            #region 监听Curd操作
            if (dbConfig.Curd)
            {
                fsql.Aop.CurdBefore += (s, e) =>
                {
                    Console.WriteLine($"{e.Sql}\r\n");
                };
            }
            #endregion

            #region 审计数据
            //计算服务器时间
            var serverTime = fsql.Select<DualEntity>().Limit(1).First(a => DateTime.UtcNow);
            var timeOffset = DateTime.UtcNow.Subtract(serverTime);
            var user = services.BuildServiceProvider().GetService<IUser>();
            fsql.Aop.AuditValue += (s, e) =>
            {
                if (user == null || user.Id <= 0)
                {
                    return;
                }

                if (e.AuditValueType == FreeSql.Aop.AuditValueType.Insert)
                {
                    switch (e.Property.Name)
                    {
                        case "CreatedUserId":
                            e.Value = user.Id;
                            break;
                        case "CreatedUserName":
                            e.Value = user.Name;
                            break;
                        case "TenantId":
                            e.Value = user.TenantId;
                            break;
                    }

                    if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                    && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
                    {
                        e.Value = DateTime.Now.Subtract(timeOffset);
                    }
                }
                else if (e.AuditValueType == FreeSql.Aop.AuditValueType.Update)
                {
                    switch (e.Property.Name)
                    {
                        case "ModifiedUserId":
                            e.Value = user.Id;
                            break;
                        case "ModifiedUserName":
                            e.Value = user.Name;
                            break;
                    }
                    if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                    && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
                    {
                        e.Value = DateTime.Now.Subtract(timeOffset);
                    }
                }
            };
            #endregion
            #endregion

            //导入多数据库
            if(null != dbConfig.Dbs)
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
