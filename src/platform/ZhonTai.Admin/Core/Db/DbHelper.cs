using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using Yitter.IdGenerator;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Auth;
using System.IO;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Db.Data;
using StackExchange.Profiling;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Startup;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.User;

namespace ZhonTai.Admin.Core.Db;

public class DbHelper
{
    /// <summary>
    /// 偏移时间
    /// </summary>
    public static TimeSpan TimeOffset;

    /// <summary>
    /// 创建数据库
    /// </summary>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    public async static Task CreateDatabaseAsync(DbConfig dbConfig)
    {
        if (!dbConfig.CreateDb || dbConfig.Type == DataType.Sqlite)
        {
            return;
        }

        var db = new FreeSqlBuilder()
                .UseConnectionString(dbConfig.Type, dbConfig.CreateDbConnectionString)
                .Build();

        try
        {
            Console.WriteLine($"{Environment.NewLine} create database started");
            var filePath = Path.Combine(AppContext.BaseDirectory, "Configs/createdbsql.txt").ToPath();
            if (File.Exists(filePath))
            {
                var createDbSql = FileHelper.ReadFile(filePath);
                if (createDbSql.NotNull())
                {
                    dbConfig.CreateDbSql = createDbSql;
                }
            }

            await db.Ado.ExecuteNonQueryAsync(dbConfig.CreateDbSql);
            Console.WriteLine(" create database succeed");
        }
        catch (Exception e)
        {
            Console.WriteLine($" create database failed.\n {e.Message}");
        }
    }

    /// <summary>
    /// 获得指定程序集表实体
    /// </summary>
    /// <param name="assemblyNames"></param>
    /// <returns></returns>
    public static Type[] GetEntityTypes(string[] assemblyNames)
    {
        if(!(assemblyNames?.Length > 0))
        {
            return null;
        }

        Assembly[]  assemblies = DependencyContext.Default.RuntimeLibraries
            .Where(a => assemblyNames.Contains(a.Name))
            .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

        var entityTypes = new List<Type>();

        foreach (var assembly in assemblies)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                foreach (Attribute attribute in type.GetCustomAttributes())
                {
                    if (attribute is TableAttribute tableAttribute)
                    {
                        if (tableAttribute.DisableSyncStructure == false)
                        {
                            entityTypes.Add(type);
                        }
                    }
                }
            }
        }

        return entityTypes.ToArray();
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    /// <param name="db"></param>
    /// <param name="appConfig"></param>
    /// <param name="dbConfig"></param>
    public static void ConfigEntity(IFreeSql db, AppConfig appConfig = null, DbConfig dbConfig = null)
    {
        //租户生成和操作租户Id
        if (!appConfig.Tenant)
        {
            var iTenant = nameof(ITenant);
            var tenantId = nameof(ITenant.TenantId);

            //获得指定程序集表实体
            var entityTypes = GetEntityTypes(dbConfig.AssemblyNames);

            foreach (var entityType in entityTypes)
            {
                if (entityType.GetInterfaces().Any(a => a.Name == iTenant))
                {
                    db.CodeFirst.Entity(entityType, a =>
                    {
                        a.Ignore(tenantId);
                    });
                }
            }
        }
    }

    /// <summary>
    /// 审计数据
    /// </summary>
    /// <param name="e"></param>
    /// <param name="timeOffset"></param>
    /// <param name="user"></param>
    public static void AuditValue(AuditValueEventArgs e, TimeSpan timeOffset, IUser user)
    {
        //数据库时间
        if ((e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
        && e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null
        && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
        {
            e.Value = DateTime.Now.Subtract(timeOffset);
        }

        //雪花Id
        if (e.Column.CsType == typeof(long)
        && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) is SnowflakeAttribute snowflakeAttribute
        && snowflakeAttribute.Enable && (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
        {
            e.Value = YitIdHelper.NextId();
        }

        //有序Guid
        if (e.Column.CsType == typeof(Guid)
        && e.Property.GetCustomAttribute<OrderGuidAttribute>(false) is OrderGuidAttribute orderGuidAttribute
        && orderGuidAttribute.Enable && (e.Value == null || (Guid)e.Value == default || (Guid?)e.Value == default))
        {
            e.Value = FreeUtil.NewMongodbId();
        }

        if (user == null || user.Id <= 0)
        {
            return;
        }

        if (e.AuditValueType == AuditValueType.Insert || e.AuditValueType == AuditValueType.InsertOrUpdate)
        {
            switch (e.Property.Name)
            {
                case "CreatedUserId":
                case "OwnerId":
                case "MemberId":
                    if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                    {
                        e.Value = user.Id;
                    }
                    break;

                case "CreatedUserName":
                    if (e.Value == null || ((string)e.Value).IsNull())
                    {
                        e.Value = user.UserName;
                    }
                    break;
                case "OwnerOrgId":
                    if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                    {
                        e.Value = user.DataPermission?.OrgId;
                    }
                    break;
                case "TenantId":
                    if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                    {
                        e.Value = user.TenantId;
                    }
                    break;

            }
        }
        else if (e.AuditValueType == AuditValueType.Update || e.AuditValueType == AuditValueType.InsertOrUpdate)
        {
            switch (e.Property.Name)
            {
                case "ModifiedUserId":
                    e.Value = user.Id;
                    break;

                case "ModifiedUserName":
                    e.Value = user.UserName;
                    break;
            }
        }
    }

    /// <summary>
    /// 同步结构
    /// </summary>
    public static void SyncStructure(IFreeSql db, string msg = null, DbConfig dbConfig = null, AppConfig appConfig = null)
    {
        //打印结构比对脚本
        //var dDL = db.CodeFirst.GetComparisonDDLStatements<PermissionEntity>();
        //Console.WriteLine($"{Environment.NewLine} " + dDL);

        //打印结构同步脚本
        //db.Aop.SyncStructureAfter += (s, e) =>
        //{
        //    if (e.Sql.NotNull())
        //    {
        //        Console.WriteLine(" sync structure sql:\n" + e.Sql);
        //    }
        //};

        // 同步结构
        var dbType = dbConfig.Type.ToString();
        Console.WriteLine($"{Environment.NewLine} {(msg.NotNull() ? msg : $"sync {dbType} structure")} started");

        if (dbConfig.Type == DataType.Oracle)
        {
            db.CodeFirst.IsSyncStructureToUpper = true;
        }

        //获得指定程序集表实体
        var entityTypes = GetEntityTypes(dbConfig.AssemblyNames);
        db.CodeFirst.SyncStructure(entityTypes);

        Console.WriteLine($" {(msg.NotNull() ? msg : $"sync {dbType} structure")} succeed");
    }

    /// <summary>
    /// 同步数据审计方法
    /// </summary>
    /// <param name="s"></param>
    /// <param name="e"></param>
    private static void SyncDataAuditValue(object s, AuditValueEventArgs e)
    {
        var user = new { Id = 161223411986501, Name = "admin", TenantId = 161223412138053 };

        if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null
               && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
               && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
        {
            e.Value = DateTime.Now.Subtract(TimeOffset);
        }

        if (e.Column.CsType == typeof(long)
        && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) != null
        && (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
        {
            e.Value = YitIdHelper.NextId();
        }

        if (user == null || user.Id <= 0)
        {
            return;
        }

        if (e.AuditValueType == AuditValueType.Insert || e.AuditValueType == AuditValueType.InsertOrUpdate)
        {
            switch (e.Property.Name)
            {
                case "CreatedUserId":
                    if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                    {
                        e.Value = user.Id;
                    }
                    break;

                case "CreatedUserName":
                    if (e.Value == null || ((string)e.Value).IsNull())
                    {
                        e.Value = user.Name;
                    }
                    break;

                case "TenantId":
                    if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                    {
                        e.Value = user.TenantId;
                    }
                    break;
            }
        }
        else if (e.AuditValueType == AuditValueType.Update || e.AuditValueType == AuditValueType.InsertOrUpdate)
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
        }
    }

    /// <summary>
    /// 同步数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="dbConfig"></param>
    /// <param name="appConfig"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task SyncDataAsync(
        IFreeSql db, 
        DbConfig dbConfig = null, 
        AppConfig appConfig = null
    )
    {
        try
        {
            Console.WriteLine($"{Environment.NewLine} sync data started");

            if (dbConfig.AssemblyNames?.Length > 0)
            {
                db.Aop.AuditValue += SyncDataAuditValue;

                Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
                .Where(a => dbConfig.AssemblyNames.Contains(a.Name))
                .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

                List<ISyncData> syncDatas = assemblies.Select(assembly => assembly.GetTypes()
                .Where(x => typeof(ISyncData).GetTypeInfo().IsAssignableFrom(x.GetTypeInfo()) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract))
                .SelectMany(registerTypes => registerTypes.Select(registerType => (ISyncData)Activator.CreateInstance(registerType))).ToList();

                foreach (ISyncData syncData in syncDatas)
                {
                    await syncData.SyncDataAsync(db, dbConfig, appConfig);
                }

                db.Aop.AuditValue -= SyncDataAuditValue;
            }

            Console.WriteLine($" sync data succeed{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            throw new Exception($" sync data failed.\n{ex.Message}");
        }
    }

    /// <summary>
    /// 生成数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="appConfig"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task GenerateDataAsync(IFreeSql db, AppConfig appConfig = null, DbConfig dbConfig = null)
    {
        try
        {
            Console.WriteLine($"{Environment.NewLine} generate data started");

            if (dbConfig.AssemblyNames?.Length > 0)
            {
                Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
               .Where(a => dbConfig.AssemblyNames.Contains(a.Name))
               .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

                List<IGenerateData> generateDatas = assemblies.Select(assembly => assembly.GetTypes()
                .Where(x => typeof(IGenerateData).GetTypeInfo().IsAssignableFrom(x.GetTypeInfo()) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract))
                .SelectMany(registerTypes => registerTypes.Select(registerType => (IGenerateData)Activator.CreateInstance(registerType))).ToList();

                foreach (IGenerateData generateData in generateDatas)
                {
                    await generateData.GenerateDataAsync(db, appConfig);
                }
            }

            Console.WriteLine($" generate data succeed{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            throw new Exception($" generate data failed。\n{ex.Message}{Environment.NewLine}");
        }
    }

    /// <summary>
    /// 注册数据库
    /// </summary>
    /// <param name="freeSqlCloud"></param>
    /// <param name="user"></param>
    /// <param name="dbConfig"></param>
    /// <param name="appConfig"></param>
    /// <param name="hostAppOptions"></param>
    public static void RegisterDb(
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
                CreateDatabaseAsync(dbConfig).Wait();
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

            //生成数据
            if (dbConfig.GenerateData && !dbConfig.CreateDb && !dbConfig.SyncData)
            {
                GenerateDataAsync(fsql, appConfig, dbConfig).Wait();
            }

            //软删除过滤器
            fsql.GlobalFilter.ApplyOnly<IDelete>(FilterNames.Delete, a => a.IsDeleted == false);

            //租户过滤器
            if (appConfig.Tenant)
            {
                fsql.GlobalFilter.ApplyOnly<ITenant>(FilterNames.Tenant, a => a.TenantId == user.TenantId);
            }

            //会员过滤器
            fsql.GlobalFilter.ApplyOnly<IMember>(FilterNames.Member, a => a.MemberId == user.Id);

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
            ConfigEntity(fsql, appConfig, dbConfig);

            hostAppOptions?.ConfigureFreeSql?.Invoke(fsql);

            #region 初始化数据库

            //同步结构
            if (dbConfig.SyncStructure)
            {
                SyncStructure(fsql, dbConfig: dbConfig, appConfig: appConfig);
            }

            #region 审计数据

            //计算服务器时间
            var serverTime = fsql.Ado.QuerySingle(() => DateTime.UtcNow);
            var timeOffset = DateTime.UtcNow.Subtract(serverTime);
            TimeOffset = timeOffset;
            fsql.Aop.AuditValue += (s, e) =>
            {
                AuditValue(e, timeOffset, user);
            };

            #endregion 审计数据

            //同步数据
            if (dbConfig.SyncData)
            {
                SyncDataAsync(fsql, dbConfig, appConfig).Wait();
            }

            #endregion 初始化数据库

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
}