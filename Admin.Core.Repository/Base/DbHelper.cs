using Admin.Core.Common.Attributes;
using Admin.Core.Common.Auth;
using Admin.Core.Common.BaseModel;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Extensions;
using Admin.Core.Common.Helpers;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin.Output;
using Admin.Core.Repository.Admin.Permission.Output;
using Admin.Core.Repository.Admin.View.Output;
using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace Admin.Core.Repository
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
        /// <returns></returns>
        public static Type[] GetEntityTypes()
        {
            List<string> assemblyNames = new List<string>()
            {
                "Admin.Core.Model"
            };

            List<Type> entityTypes = new List<Type>();

            foreach (var assemblyName in assemblyNames)
            {
                foreach (Type type in Assembly.Load(assemblyName).GetExportedTypes())
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
                var entityTypes = GetEntityTypes();

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
            var entityTypes = GetEntityTypes();

            db.CodeFirst.SyncStructure(entityTypes);
            Console.WriteLine($" {(msg.NotNull() ? msg : $"sync {dbType} structure")} succeed");
        }

        /// <summary>
        /// 检查实体属性是否为自增长
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static bool CheckIdentity<T>() where T : class
        {
            var isIdentity = false;
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault() is ColumnAttribute columnAttribute && columnAttribute.IsIdentity)
                {
                    isIdentity = true;
                    break;
                }
            }

            return isIdentity;
        }

        /// <summary>
        /// 初始化数据表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="tran"></param>
        /// <param name="data"></param>
        /// <param name="dbConfig"></param>
        /// <returns></returns>
        private static async Task InitDtDataAsync<T>(
            IFreeSql db,
            IUnitOfWork unitOfWork,
            System.Data.Common.DbTransaction tran,
            T[] data,
            DbConfig dbConfig = null
        ) where T : class
        {
            var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
            var tableName = table.Name;

            try
            {
                if (await db.Queryable<T>().AnyAsync())
                {
                    Console.WriteLine($" table: {tableName} record already exists");
                    return;
                }

                if (!(data?.Length > 0))
                {
                    Console.WriteLine($" table: {tableName} import data []");
                    return;
                }

                var repo = db.GetRepository<T>();
                var insert = db.Insert<T>();
                if (unitOfWork != null)
                {
                    repo.UnitOfWork = unitOfWork;
                    insert = insert.WithTransaction(tran);
                }

                var isIdentity = CheckIdentity<T>();
                if (isIdentity)
                {
                    if (dbConfig.Type == DataType.SqlServer)
                    {
                        var insrtSql = insert.AppendData(data).InsertIdentity().ToSql();
                        await repo.Orm.Ado.ExecuteNonQueryAsync($"SET IDENTITY_INSERT {tableName} ON\n {insrtSql} \nSET IDENTITY_INSERT {tableName} OFF");
                    }
                    else
                    {
                        await insert.AppendData(data).InsertIdentity().ExecuteAffrowsAsync();
                    }
                }
                else
                {
                    repo.DbContextOptions.EnableAddOrUpdateNavigateList = true;
                    await repo.InsertAsync(data);
                }

                Console.WriteLine($" table: {tableName} sync data succeed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" table: {tableName} sync data failed.\n{ex.Message}");
            }
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
        /// <returns></returns>
        public static async Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null)
        {
            try
            {
                //db.Aop.CurdBefore += (s, e) =>
                //{
                //    Console.WriteLine($"{e.Sql}\r\n");
                //};

                Console.WriteLine("\r\n sync data started");

                db.Aop.AuditValue += SyncDataAuditValue;

                var fileName = appConfig.Tenant ? "data-share.json" : "data.json";
                var filePath = Path.Combine(AppContext.BaseDirectory, $"Db/Data/{fileName}").ToPath();
                var jsonData = FileHelper.ReadFile(filePath);
                var data = JsonConvert.DeserializeObject<Data>(jsonData);

                using (var uow = db.CreateUnitOfWork())
                using (var tran = uow.GetOrBeginTransaction())
                {
                    var dualRepo = db.GetRepository<DualEntity>();
                    dualRepo.UnitOfWork = uow;
                    if (!await dualRepo.Select.AnyAsync())
                    {
                        await dualRepo.InsertAsync(new DualEntity { });
                    }

                    //await InitDtDataAsync(db, uow, tran, data.Dictionaries, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.ApiTree, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.ViewTree, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.PermissionTree, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.OrganizationTree, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.Users, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.Roles, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.UserRoles, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.RolePermissions, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.Tenants, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.TenantPermissions, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.PermissionApis, dbConfig);

                    uow.Commit();
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
        /// 生成极简数据
        /// </summary>
        /// <param name="db"></param>
        /// <param name="appConfig"></param>
        /// <returns></returns>
        public static async Task GenerateSimpleJsonDataAsync(IFreeSql db, AppConfig appConfig = null)
        {
            try
            {
                Console.WriteLine("\r\n generate data started");

                #region 数据表

                #region 数据字典

                //var dictionaries = await db.Queryable<DictionaryEntity>().ToListAsync(a => new
                //{
                //    a.TenantId,
                //    a.Id,
                //    a.ParentId,
                //    a.Name,
                //    a.Code,
                //    a.Value,
                //    a.Description,
                //    a.Sort
                //});

                #endregion

                #region 接口

                var apis = await db.Queryable<ApiEntity>().ToListAsync<ApiDataOutput>();
                var apiTree = apis.ToTree((r, c) =>
                {
                    return c.ParentId == 0;
                },
                (r, c) =>
                {
                    return r.Id == c.ParentId;
                },
                (r, datalist) =>
                {
                    r.Childs ??= new List<ApiDataOutput>();
                    r.Childs.AddRange(datalist);
                });

                #endregion

                #region 视图

                var views = await db.Queryable<ViewEntity>().ToListAsync<ViewDataOutput>();
                var viewTree = views.ToTree((r, c) =>
                {
                    return c.ParentId == 0;
                },
               (r, c) =>
               {
                   return r.Id == c.ParentId;
               },
               (r, datalist) =>
               {
                   r.Childs ??= new List<ViewDataOutput>();
                   r.Childs.AddRange(datalist);
               });

                #endregion

                #region 权限

                var permissions = await db.Queryable<PermissionEntity>().ToListAsync<PermissionDataOutput>();
                var permissionTree = permissions.ToTree((r, c) =>
                {
                    return c.ParentId == 0;
                },
               (r, c) =>
               {
                   return r.Id == c.ParentId;
               },
               (r, datalist) =>
               {
                   r.Childs ??= new List<PermissionDataOutput>();
                   r.Childs.AddRange(datalist);
               });

                #endregion

                #region 组织机构

                var organizations = await db.Queryable<OrganizationEntity>().ToListAsync<OrganizationDataOutput>();
                var organizationTree = organizations.ToTree((r, c) =>
                {
                    return c.ParentId == 0;
                },
                (r, c) =>
                {
                    return r.Id == c.ParentId;
                },
                (r, datalist) =>
                {
                    r.Childs ??= new List<OrganizationDataOutput>();
                    r.Childs.AddRange(datalist);
                });

                #endregion

                #region 用户

                var users = await db.Queryable<UserEntity>().ToListAsync(a => new
                {
                    a.TenantId,
                    a.Id,
                    a.UserName,
                    a.Password,
                    a.NickName,
                    a.Avatar,
                    a.Status,
                    a.Remark
                });

                #endregion

                #region 角色

                var roles = await db.Queryable<RoleEntity>().ToListAsync(a => new
                {
                    a.TenantId,
                    a.Id,
                    a.Name,
                    a.Code,
                    a.Sort,
                    a.Description
                });

                #endregion

                #region 用户角色

                var userRoles = await db.Queryable<UserRoleEntity>().ToListAsync(a => new
                {
                    a.Id,
                    a.UserId,
                    a.RoleId
                });

                #endregion

               

                #region 角色权限

                var rolePermissions = await db.Queryable<RolePermissionEntity>().ToListAsync(a => new
                {
                    a.Id,
                    a.RoleId,
                    a.PermissionId
                });

                #endregion

                #region 租户

                var tenants = await db.Queryable<TenantEntity>().ToListAsync(a => new
                {
                    a.TenantId,
                    a.Id,
                    a.UserId,
                    a.RoleId,
                    a.Name,
                    a.Code,
                    a.RealName,
                    a.Phone,
                    a.Email,
                    a.TenantType,
                    a.DataIsolationType,
                    a.DbType,
                    a.ConnectionString,
                    a.IdleTime,
                    a.Description
                });

                #endregion

                #region 租户权限

                var tenantPermissions = await db.Queryable<TenantPermissionEntity>().ToListAsync(a => new
                {
                    a.Id,
                    a.TenantId,
                    a.PermissionId
                });

                #endregion

                #region 权限接口

                var permissionApis = await db.Queryable<PermissionApiEntity>().ToListAsync(a => new
                {
                    a.Id,
                    a.PermissionId,
                    a.ApiId
                });

                #endregion

                #endregion

                if (!(users?.Count > 0))
                {
                    return;
                }

                #region 生成数据

                var settings = new JsonSerializerSettings();
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                var jsonData = JsonConvert.SerializeObject(new
                {
                    //dictionaries,
                    apis,
                    apiTree,
                    viewTree,
                    permissionTree,
                    users,
                    roles,
                    userRoles,
                    rolePermissions,
                    tenants,
                    tenantPermissions,
                    permissionApis
                },
                //Formatting.Indented,
                settings
                );

                var fileName = appConfig.Tenant ? "data-share.json" : "data.json";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"Db/Data/{fileName}").ToPath();
                FileHelper.WriteFile(filePath, jsonData);

                #endregion

                Console.WriteLine(" generate data succeed\r\n");
            }
            catch (Exception ex)
            {
                throw new Exception($" generate data failed。\n{ex.Message}\r\n");
            }
        }
    }
}