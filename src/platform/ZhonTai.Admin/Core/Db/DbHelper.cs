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

namespace ZhonTai.Admin.Core.Db
{
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
                Console.WriteLine("\r\n create database started");
                var filePath = Path.Combine(AppContext.BaseDirectory, "Configs/createdbsql.txt").ToPath();
                if (File.Exists(filePath))
                {
                    dbConfig.CreateDbSql = FileHelper.ReadFile(filePath);
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
        /// <param name="appConfig"></param>
        /// <returns></returns>
        public static Type[] GetEntityTypes(AppConfig appConfig)
        {
            Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
                .Where(a => appConfig.AssemblyNames.Contains(a.Name) || a.Name == "ZhonTai.Admin")
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
        public static void ConfigEntity(IFreeSql db, AppConfig appConfig = null)
        {
            //租户生成和操作租户Id
            if (!appConfig.Tenant)
            {
                var iTenant = nameof(ITenant);
                var tenantId = nameof(ITenant.TenantId);

                //获得指定程序集表实体
                var entityTypes = GetEntityTypes(appConfig);

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
            if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null
                   && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                   && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
            {
                e.Value = DateTime.Now.Subtract(timeOffset);
            }

            if (e.Column.CsType == typeof(long)
            && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) is SnowflakeAttribute snowflakeAttribute
            && snowflakeAttribute.Enable && (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
            {
                e.Value = YitIdHelper.NextId();
            }

            if (user == null || user.Id <= 0)
            {
                return;
            }

            if (e.AuditValueType == AuditValueType.Insert)
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
            else if (e.AuditValueType == AuditValueType.Update)
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
        /// 同步结构
        /// </summary>
        public static void SyncStructure(IFreeSql db, string msg = null, DbConfig dbConfig = null, AppConfig appConfig = null)
        {
            //打印结构比对脚本
            //var dDL = db.CodeFirst.GetComparisonDDLStatements<PermissionEntity>();
            //Console.WriteLine("\r\n " + dDL);

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
            Console.WriteLine($"\r\n {(msg.NotNull() ? msg : $"sync {dbType} structure")} started");
            if (dbConfig.Type == DataType.Oracle)
            {
                db.CodeFirst.IsSyncStructureToUpper = true;
            }

            //获得指定程序集表实体
            var entityTypes = GetEntityTypes(appConfig);

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

            if (e.AuditValueType == AuditValueType.Insert)
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
            else if (e.AuditValueType == AuditValueType.Update)
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
                Console.WriteLine("\r\n sync data started");

                db.Aop.AuditValue += SyncDataAuditValue;

                Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
                .Where(a => appConfig.AssemblyNames.Contains(a.Name) || a.Name == "ZhonTai.Admin")
                .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

                List<ISyncData> syncDatas = assemblies.Select(assembly => assembly.GetTypes()
                .Where(x => typeof(ISyncData).GetTypeInfo().IsAssignableFrom(x.GetTypeInfo()) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract))
                .SelectMany(registerTypes => registerTypes.Select(registerType => (ISyncData)Activator.CreateInstance(registerType))).ToList();

                foreach (ISyncData syncData in syncDatas)
                {
                    await syncData.SyncDataAsync(db, dbConfig, appConfig);
                }

                db.Aop.AuditValue -= SyncDataAuditValue;

                Console.WriteLine(" sync data succeed\r\n");
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
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task GenerateDataAsync(IFreeSql db, AppConfig appConfig = null)
        {
            try
            {
                Console.WriteLine("\r\n generate data started");

                Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
                .Where(a => appConfig.AssemblyNames.Contains(a.Name) || a.Name == "ZhonTai.Admin")
                .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

                List<IGenerateData> generateDatas = assemblies.Select(assembly => assembly.GetTypes()
                .Where(x => typeof(IGenerateData).GetTypeInfo().IsAssignableFrom(x.GetTypeInfo()) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract))
                .SelectMany(registerTypes => registerTypes.Select(registerType => (IGenerateData)Activator.CreateInstance(registerType))).ToList();

                foreach (IGenerateData generateData in generateDatas)
                {
                    await generateData.GenerateDataAsync(db, appConfig);
                }

                Console.WriteLine(" generate data succeed\r\n");
            }
            catch (Exception ex)
            {
                throw new Exception($" generate data failed。\n{ex.Message}\r\n");
            }
        }
    }
}