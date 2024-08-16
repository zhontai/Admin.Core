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
    private static TimeSpan timeOffset;

    public static TimeSpan TimeOffset { get => timeOffset; set => timeOffset = value; }

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
            Console.WriteLine($"{Environment.NewLine}create database started");
            var filePath = Path.Combine(AppContext.BaseDirectory, dbConfig.CreateDbSqlFile).ToPath();
            if (File.Exists(filePath))
            {
                var createDbSql = FileHelper.ReadFile(filePath);
                if (createDbSql.NotNull())
                {
                    dbConfig.CreateDbSql = createDbSql;
                }
            }

            await db.Ado.ExecuteNonQueryAsync(dbConfig.CreateDbSql);
            Console.WriteLine("create database succeed");
        }
        catch (Exception e)
        {
            Console.WriteLine($"create database failed.\n {e.Message}");
        }
    }

    /// <summary>
    /// 获得指定程序集表实体
    /// </summary>
    /// <param name="assemblyNames"></param>
    /// <returns></returns>
    public static Type[] GetEntityTypes(string[] assemblyNames)
    {
        if (!(assemblyNames?.Length > 0))
        {
            return null;
        }

        var entityTypes = new List<Type>();

        foreach (var assemblyName in assemblyNames)
        {
            var assembly = Assembly.Load(assemblyName);
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
    /// <param name="dbConfig"></param>
    public static void AuditValue(AuditValueEventArgs e, TimeSpan timeOffset, IUser user, DbConfig dbConfig)
    {
        if (e.Property == null)
        {
            return;
        }

        //数据库时间
        if ((e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
        && e.Property.GetCustomAttribute<ServerTimeAttribute>(false) is ServerTimeAttribute serverTimeAttribute)
        {
            if(!dbConfig.ForceUpdate && !serverTimeAttribute.CanInsert && e.AuditValueType is AuditValueType.Insert)
            {
                return;
            }

            if((e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default) || serverTimeAttribute.CanUpdate)
            {
                e.Value = DateTime.Now.Subtract(timeOffset);
            }
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

        if (e.AuditValueType is AuditValueType.Insert or AuditValueType.InsertOrUpdate)
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
                case "CreatedUserRealName":
                    if (e.Value == null || ((string)e.Value).IsNull())
                    {
                        e.Value = user.Name;
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

        if ((e.AuditValueType is AuditValueType.Update or AuditValueType.InsertOrUpdate) || dbConfig.ForceUpdate)
        {
            switch (e.Property.Name)
            {
                case "ModifiedUserId":
                    e.Value = user.Id;
                    break;
                case "ModifiedUserName":
                    e.Value = user.UserName;
                    break;
                case "ModifiedUserRealName":
                    e.Value = user.Name;
                    break;
            }
        }
    }

    private static void SyncStructureAfter(object? s, SyncStructureAfterEventArgs e)
    {
        if (e.Sql.NotNull())
        {
            Console.WriteLine("sync structure sql:\n" + e.Sql);
        }
    }

    /// <summary>
    /// 同步结构
    /// </summary>
    /// <param name="db"></param>
    /// <param name="msg"></param>
    /// <param name="dbConfig"></param>
    /// <param name="configureFreeSqlSyncStructure"></param>
    public static void SyncStructure(IFreeSql db, string msg = null, DbConfig dbConfig = null, Action<IFreeSql, DbConfig> configureFreeSqlSyncStructure = null)
    {
        //打印结构比对脚本
        //var dDL = db.CodeFirst.GetComparisonDDLStatements<PermissionEntity>();
        //Console.WriteLine($"{Environment.NewLine}" + dDL);

        //打印结构同步脚本
        if (dbConfig.SyncStructureSql)
        {
            db.Aop.SyncStructureAfter += SyncStructureAfter;
        }

        // 同步结构
        var dbType = dbConfig.Type.ToString();
        Console.WriteLine($"{Environment.NewLine}{(msg.NotNull() ? msg : $"sync {dbType} structure")} started");

        //获得指定程序集表实体
        var entityTypes = GetEntityTypes(dbConfig.AssemblyNames)?.ToList();

        var batchSize = dbConfig.SyncStructureEntityBatchSize;
        batchSize = batchSize <= 1 ? 1 : batchSize;

        if(entityTypes != null && entityTypes.Count > 0)
        {
            if (batchSize == 1)
            {
                foreach (var entityType in entityTypes)
                {
                    db.CodeFirst.SyncStructure(entityType);
                }
            }
            else
            {
                for (int i = 0, count = entityTypes.Count; i < count; i += batchSize)
                {
                    var batchEntityTypes = entityTypes.GetRange(i, Math.Min(batchSize, count - i));
                    db.CodeFirst.SyncStructure(batchEntityTypes.ToArray());
                }
            }
        }

        //自定义迁移结构
        configureFreeSqlSyncStructure?.Invoke(db, dbConfig);

        if (dbConfig.SyncStructureSql)
        {
            db.Aop.SyncStructureAfter -= SyncStructureAfter;
        }

        Console.WriteLine($"{(msg.NotNull() ? msg : $"sync {dbType} structure")} succeed");
    }

    private static void SyncDataCurdBefore(object? s, CurdBeforeEventArgs e)
    {
        if (e.Sql.NotNull())
        {
            Console.WriteLine($"{e.Sql}{Environment.NewLine}");
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
            Console.WriteLine($"{Environment.NewLine}sync data started");

            if (dbConfig.AssemblyNames?.Length > 0)
            {
                var user = dbConfig.SyncDataUser;

                // 同步数据审计方法
                void SyncDataAuditValue(object s, AuditValueEventArgs e)
                {
                    if (e.Property == null)
                    {
                        return;
                    }

                    if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) is ServerTimeAttribute serverTimeAttribute
                           && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                           && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
                    {
                        if (!dbConfig.ForceUpdate && !serverTimeAttribute.CanInsert && e.AuditValueType is AuditValueType.Insert)
                        {
                            return;
                        }

                        if ((e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default) || serverTimeAttribute.CanUpdate)
                        {
                            e.Value = DateTime.Now.Subtract(timeOffset);
                        }
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

                    if (e.AuditValueType is AuditValueType.Insert or AuditValueType.InsertOrUpdate)
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
                                    e.Value = user.UserName;
                                }
                                break;
                            case "CreatedUserRealName":
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

                    if ((e.AuditValueType is AuditValueType.Update or AuditValueType.InsertOrUpdate) || dbConfig.ForceUpdate)
                    {
                        switch (e.Property.Name)
                        {
                            case "ModifiedUserId":
                                if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                                {
                                    e.Value = user.Id;
                                }
                                break;
                            case "ModifiedUserName":
                                if (e.Value == null || ((string)e.Value).IsNull())
                                {
                                    e.Value = user.UserName;
                                }
                                break;
                            case "ModifiedUserRealName":
                                if (e.Value == null || ((string)e.Value).IsNull())
                                {
                                    e.Value = user.Name;
                                }
                                break;
                        }
                    }
                }

                db.Aop.AuditValue += SyncDataAuditValue;

                if (dbConfig.SyncDataCurd)
                {
                    db.Aop.CurdBefore += SyncDataCurdBefore;
                }

                Assembly[] assemblies = AssemblyHelper.GetAssemblyList(dbConfig.AssemblyNames);

                List<ISyncData> syncDatas = assemblies.Select(assembly => assembly.GetTypes()
                .Where(x => typeof(ISyncData).GetTypeInfo().IsAssignableFrom(x.GetTypeInfo()) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract))
                .SelectMany(registerTypes => registerTypes.Select(registerType => (ISyncData)Activator.CreateInstance(registerType))).ToList();

                foreach (ISyncData syncData in syncDatas)
                {
                    await syncData.SyncDataAsync(db, dbConfig, appConfig);
                }

                if (dbConfig.SyncDataCurd)
                {
                    db.Aop.CurdBefore -= SyncDataCurdBefore;
                }

                db.Aop.AuditValue -= SyncDataAuditValue;
            }

            Console.WriteLine($"sync data succeed{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            throw new Exception($"sync data failed.\n{ex.Message}");
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
            Console.WriteLine($"{Environment.NewLine}generate data started");

            if (dbConfig.AssemblyNames?.Length > 0)
            {
                Assembly[] assemblies = AssemblyHelper.GetAssemblyList(dbConfig.AssemblyNames);

                List<IGenerateData> generateDatas = assemblies.Select(assembly => assembly.GetTypes()
                .Where(x => typeof(IGenerateData).GetTypeInfo().IsAssignableFrom(x.GetTypeInfo()) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract))
                .SelectMany(registerTypes => registerTypes.Select(registerType => (IGenerateData)Activator.CreateInstance(registerType))).ToList();

                foreach (IGenerateData generateData in generateDatas)
                {
                    await generateData.GenerateDataAsync(db, appConfig);
                }
            }

            Console.WriteLine($"generate data succeed{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            throw new Exception($"generate data failed。\n{ex.Message}{Environment.NewLine}");
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
        var idelTime = dbConfig.IdleTime.HasValue && dbConfig.IdleTime.Value > 0 ? TimeSpan.FromMinutes(dbConfig.IdleTime.Value) : TimeSpan.MaxValue;
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

            hostAppOptions?.ConfigureFreeSqlBuilder?.Invoke(freeSqlBuilder, dbConfig);

            var fsql = freeSqlBuilder.Build();

            //生成数据
            if (dbConfig.GenerateData && !dbConfig.CreateDb && !dbConfig.SyncData)
            {
                GenerateDataAsync(fsql, appConfig, dbConfig).Wait();
            }

            //计算服务器时间
            var serverTime = fsql.Ado.QuerySingle(() => DateTime.UtcNow);
            var timeOffset = DateTime.UtcNow.Subtract(serverTime);
            TimeOffset = timeOffset;

            if (dbConfig.Type == DataType.Oracle)
            {
                fsql.CodeFirst.IsSyncStructureToUpper = true;
            }

            //同步结构
            if (dbConfig.SyncStructure)
            {
                SyncStructure(fsql, dbConfig: dbConfig, configureFreeSqlSyncStructure: hostAppOptions?.ConfigureFreeSqlSyncStructure);
            }

            //同步数据
            if (dbConfig.SyncData)
            {
                SyncDataAsync(fsql, dbConfig, appConfig).Wait();
            }

            //审计数据
            fsql.Aop.AuditValue += (s, e) =>
            {
                AuditValue(e, timeOffset, user, dbConfig);
            };

            #region 过滤器

            //软删除过滤器
            fsql.GlobalFilter.ApplyOnly<IDelete>(FilterNames.Delete, a => a.IsDeleted == false);

            //租户过滤器
            if (appConfig.Tenant)
            {
                fsql.GlobalFilter.ApplyOnly<ITenant>(FilterNames.Tenant, a => a.TenantId == user.TenantId);
            }

            //会员过滤器
            fsql.GlobalFilter.ApplyOnlyIf<IMember>(FilterNames.Member,
                () =>
                {
                    if (user?.Id > 0 && user.Type != UserType.Member)
                    {
                        return false;
                    }
                    return true;
                },
                a => a.MemberId == user.Id
            );

            //数据权限过滤器
            fsql.GlobalFilter.ApplyOnlyIf<IData>(FilterNames.Self,
                () =>
                {
                    var dataPermission = user.DataPermission;
                    if (dataPermission != null && (dataPermission.DataScope == DataScope.All || dataPermission.OrgIds.Count > 0))
                    {
                        return false;
                    }
                    return true;
                },
                a => a.OwnerId == user.Id
            );
            fsql.GlobalFilter.ApplyOnlyIf<IData>(FilterNames.Data,
                () =>
                {
                    var dataPermission = user.DataPermission;
                    if (dataPermission == null || (dataPermission != null && (dataPermission.DataScope == DataScope.All || dataPermission.OrgIds.Count == 0)))
                    {
                        return false;
                    }
                    return true;
                },
                a => a.OwnerId == user.Id || user.DataPermission.OrgIds.Contains(a.OwnerOrgId.Value)
            );

            #endregion

            //配置实体
            ConfigEntity(fsql, appConfig, dbConfig);

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

            hostAppOptions?.ConfigureFreeSql?.Invoke(fsql, dbConfig);

            return fsql;
        }, idelTime);
    }
}