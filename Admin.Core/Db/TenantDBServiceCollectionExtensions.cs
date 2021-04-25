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
            if(appConfig.TenantType == TenantType.Own)
            {
                tenantName = "tenant_" + user.TenantId.ToString();
            }
            ib.TryRegister(tenantName, () =>
            {
                #region FreeSql
                var freeSqlBuilder = new FreeSqlBuilder()
                        .UseConnectionString(dbConfig.Type, dbConfig.ConnectionString)
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

                //共享数据库
                if (appConfig.TenantType == TenantType.Share)
                {
                    fsql.GlobalFilter.ApplyIf<ITenant>("Tenant", () => user.TenantId > 0, a => a.TenantId == user.TenantId);
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

                return fsql;
            });

            services.AddSingleton(ib);
        }
    }
}
