using Microsoft.Extensions.DependencyInjection;
using System;
using StackExchange.Profiling;
using FreeSql;
using FreeSql.Internal.CommonProvider;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;

namespace ZhonTai.Admin.Core.Db;

public static class FreeSqlCloudExtesions
{
    /// <summary>
    /// 创建FreeSql实例
    /// </summary>
    /// <param name="user"></param>
    /// <param name="appConfig"></param>
    /// <param name="dbConfig"></param>
    /// <param name="tenant"></param>
    /// <returns></returns>
    private static IFreeSql CreateFreeSql(IUser user, AppConfig appConfig, DbConfig dbConfig, CreateFreeSqlTenantDto tenant)
    {
        var freeSqlBuilder = new FreeSqlBuilder()
        .UseConnectionString(tenant.DbType.Value, tenant.ConnectionString)
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

        #endregion 监听所有命令

        var fsql = freeSqlBuilder.Build();
        fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDeleted == false);

        //配置实体
        DbHelper.ConfigEntity(fsql, appConfig);

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
                Console.WriteLine($"{e.Sql}\r\n");
            };
        }

        #endregion 监听Curd操作

        #region 审计数据

        //计算服务器时间
        var selectProvider = fsql.Select<object>() as Select0Provider;
        var serverTime = fsql.Select<object>().WithSql($"select {selectProvider._commonUtils.NowUtc} a").First(a=> Convert.ToDateTime("a"));
        var timeOffset = DateTime.UtcNow.Subtract(serverTime);
        fsql.Aop.AuditValue += (s, e) =>
        {
            DbHelper.AuditValue(e, timeOffset, user);
        };

        #endregion 审计数据

        if (appConfig.Tenant)
        {
            fsql.GlobalFilter.Apply<ITenant>("Tenant", a => a.TenantId == user.TenantId);
        }

        return fsql;
    }

    /// <summary>
    /// 获得当前登录用户数据库
    /// </summary>
    /// <param name="cloud"></param>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static IFreeSql GetCurrentDb(this FreeSqlCloud cloud, IServiceProvider serviceProvider)
    {
        var user = serviceProvider.GetRequiredService<IUser>();
        var appConfig = serviceProvider.GetRequiredService<AppConfig>();

        var tenantId = user.TenantId;
        if (appConfig.Tenant && tenantId.HasValue)
        {
            var dbKey = user.DbKey.NotNull() ? user.DbKey : (DbKeys.TenantDbKey + tenantId);
            var exists = cloud.ExistsRegister(dbKey);
            if (!exists)
            {
                var dbConfig = serviceProvider.GetRequiredService<DbConfig>();
                var tenantRepository = serviceProvider.GetRequiredService<ITenantRepository>();
                var tenant = tenantRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(tenantId).ToOne<CreateFreeSqlTenantDto>();
                cloud.Register(dbKey, () => CreateFreeSql(user, appConfig, dbConfig, tenant));
            }

            return cloud.Use(dbKey);
        }
        else
        {
            var masterDb = cloud.Use(DbKeys.MasterDbKey);
            return masterDb;
        }
    }

    /// <summary>
    /// 获得租户数据库
    /// </summary>
    /// <param name="cloud"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public static IFreeSql GetTenantDb(this FreeSqlCloud cloud, IServiceProvider serviceProvider, long? tenantId = null)
    {
        if (!tenantId.HasValue)
        {
            return null;
        }

        var tenantRepository = serviceProvider.GetRequiredService<ITenantRepository>();
        var tenant = tenantRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(tenantId).ToOne<CreateFreeSqlTenantDto>();
        var dbKey = tenant.DbKey.NotNull() ? tenant.DbKey : (DbKeys.TenantDbKey + tenantId);
        var exists = cloud.ExistsRegister(dbKey);
        
        if (!exists)
        {
            var user = serviceProvider.GetRequiredService<IUser>();
            var dbConfig = serviceProvider.GetRequiredService<DbConfig>();
            var appConfig = serviceProvider.GetRequiredService<AppConfig>();
            cloud.Register(dbKey, () => CreateFreeSql(user, appConfig, dbConfig, tenant));
        }

        return cloud.Use(dbKey);
    }
}