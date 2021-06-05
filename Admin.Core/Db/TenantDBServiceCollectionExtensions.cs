using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FreeSql;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Auth;
using Admin.Core.Common.BaseModel;
using Admin.Core.Repository;
using Admin.Core.Common.Consts;
using Admin.Core.Model.Admin;
using Admin.Core.Common.Attributes;
using System.Reflection;
using Yitter.IdGenerator;

namespace Admin.Core.Db
{
    //public class MyIdleBus : IdleBus<DbName, IFreeSql>
    //{
    //    public MyIdleBus() : base(TimeSpan.FromMinutes(30)) { }
    //}

    public static class TenantDBServiceCollectionExtensions
    {
        /// <summary>
        /// 添加租户数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        public static void AddTenantDb(this IServiceCollection services, IHostEnvironment env)
        {
            services.AddScoped<MyUnitOfWorkManager>();
            
            var dbConfig = new ConfigHelper().Get<DbConfig>("dbconfig", env.EnvironmentName);
            var appConfig = new ConfigHelper().Get<AppConfig>("appconfig", env.EnvironmentName);
            var user = services.BuildServiceProvider().GetService<IUser>();

            int idleTime = dbConfig.IdleTime > 0 ? dbConfig.IdleTime : 10;

            IdleBus <IFreeSql> ib = new IdleBus<IFreeSql>(TimeSpan.FromMinutes(idleTime));

            var tenantName = AdminConsts.TenantName;
            //需要查询租户数据库类型
            //if (appConfig.TenantDbType == TenantDbType.Own)
            //{
            //    tenantName = "tenant_" + user.TenantId?.ToString();
            //}
            ib.TryRegister(tenantName, () =>
            {
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
                        Console.WriteLine($"{cmd.CommandText}\r\n");
                    });
                }
                #endregion

                var fsql = freeSqlBuilder.Build();
                fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDeleted == false);
                
                //配置实体
                DbHelper.ConfigEntity(fsql, appConfig);

                ////需要查询租户数据库类型
                //共享数据库
                //if (appConfig.TenantDbType == TenantDbType.Share)
                //{
                //    fsql.GlobalFilter.Apply<ITenant>("Tenant", a => a.TenantId == user.TenantId);
                //}

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
                fsql.Aop.AuditValue += (s, e) =>
                {
                    DbHelper.AuditValue(e, timeOffset, user);
                };
                #endregion
                #endregion

                return fsql;
            });

            services.AddSingleton(ib);
        }
    }
}
