using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Helpers;
using Admin.Core.Model.Admin;

namespace Admin.Core.Db
{
    public class TenantDbHelper
    {
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbConfig"></param>
        /// <returns></returns>
        public async static Task CreateDatabase(DbConfig dbConfig)
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
                Console.WriteLine("\r\ncreate database started");
                await db.Ado.ExecuteNonQueryAsync(dbConfig.CreateDbSql);
                Console.WriteLine("create database succeed\r\n");
            }
            catch (Exception e)
            {
                Console.WriteLine($"create database failed.\n{e.Message}\r\n");
            }
        }

        /// <summary>
        /// 同步结构
        /// </summary>
        public static void SyncStructure(IFreeSql db, string msg = null, DbConfig dbConfig = null)
        {
            //打印结构比对脚本
            //var dDL = db.CodeFirst.GetComparisonDDLStatements<PermissionEntity>();
            //Console.WriteLine("\r\n" + dDL);

            //打印结构同步脚本
            //db.Aop.SyncStructureAfter += (s, e) =>
            //{
            //    if (e.Sql.NotNull())
            //    {
            //        Console.WriteLine("sync structure sql:\n" + e.Sql);
            //    }
            //};

            // 同步结构
            var dbType = dbConfig.Type.ToString();
            Console.WriteLine($"{(msg.NotNull() ? msg : $"sync {dbType} structure")} started");
            if(dbConfig.Type == DataType.Oracle)
            {
                db.CodeFirst.IsSyncStructureToUpper = true;
            }
            db.CodeFirst.SyncStructure(new Type[]
            {
                typeof(DictionaryEntity),
                typeof(ApiEntity),
                typeof(ViewEntity),
                typeof(PermissionEntity),
                typeof(UserEntity),
                typeof(RoleEntity),
                typeof(UserRoleEntity),
                typeof(RolePermissionEntity),
                typeof(OprationLogEntity),
                typeof(LoginLogEntity),
                typeof(DocumentEntity),
                typeof(DocumentImageEntity)
            });
            Console.WriteLine($"{(msg.NotNull() ? msg : $"sync {dbType} structure")} succeed\r\n");
        }

        /// <summary>
        /// 初始化数据表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="data"></param>
        /// <param name="tran"></param>
        /// <param name="dbConfig"></param>
        /// <returns></returns>
        private static async Task InitDtData<T>(
            IFreeSql db, 
            T[] data, 
            System.Data.Common.DbTransaction tran, 
            DbConfig dbConfig = null
        ) where T : class
        {
            var table = typeof(T).GetCustomAttributes(typeof(TableAttribute),false).FirstOrDefault() as TableAttribute;
            var tableName = table.Name;

            try
            {
                if (!await db.Queryable<T>().AnyAsync())
                {
                    if (data?.Length > 0)
                    {
                        var insert = db.Insert<T>();

                        if(tran != null)
                        {
                            insert = insert.WithTransaction(tran);
                        }

                        if(dbConfig.Type == DataType.SqlServer)
                        {
                            var insrtSql = insert.AppendData(data).InsertIdentity().ToSql();
                            await db.Ado.ExecuteNonQueryAsync($"SET IDENTITY_INSERT {tableName} ON\n {insrtSql} \nSET IDENTITY_INSERT {tableName} OFF");
                        }
                        else
                        {
                            await insert.AppendData(data).InsertIdentity().ExecuteAffrowsAsync();
                        }
                        
                        Console.WriteLine($"table: {tableName} sync data succeed");
                    }
                    else
                    {
                        Console.WriteLine($"table: {tableName} import data []");
                    }
                }
                else
                {
                    Console.WriteLine($"table: {tableName} record already exists");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            }
        }

        /// <summary>
        /// 同步数据审计方法
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private static void SyncDataAuditValue(object s, AuditValueEventArgs e)
        {
            if (e.AuditValueType == AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case "CreatedUserId":
                        e.Value = 2;
                        break;
                    case "CreatedUserName":
                        e.Value = "admin";
                        break;
                }
            }
            else if (e.AuditValueType == AuditValueType.Update)
            {
                switch (e.Property.Name)
                {
                    case "ModifiedUserId":
                        e.Value = 2;
                        break;
                    case "ModifiedUserName":
                        e.Value = "admin";
                        break;
                }
            }
        }

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <returns></returns>
        public static async Task SyncData(IFreeSql db, DbConfig dbConfig = null)
        {
            try
            {
                //db.Aop.CurdBefore += (s, e) =>
                //{
                //    Console.WriteLine($"{e.Sql}\r\n");
                //};

                Console.WriteLine("sync data started");

                db.Aop.AuditValue += SyncDataAuditValue;
               
                var filePath = Path.Combine(AppContext.BaseDirectory, "Db/Data/data.json").ToPath();
                var jsonData = FileHelper.ReadFile(filePath);
                var data = JsonConvert.DeserializeObject<Data>(jsonData);

                using (var uow = db.CreateUnitOfWork())
                using (var tran = uow.GetOrBeginTransaction())
                {
                    await InitDtData(db, data.Dictionaries, tran, dbConfig);
                    await InitDtData(db, data.Apis, tran, dbConfig);
                    await InitDtData(db, data.Views, tran, dbConfig);
                    await InitDtData(db, data.Permissions, tran, dbConfig);
                    await InitDtData(db, data.Users, tran, dbConfig);
                    await InitDtData(db, data.Roles, tran, dbConfig);
                    await InitDtData(db, data.UserRoles, tran, dbConfig);
                    await InitDtData(db, data.RolePermissions, tran, dbConfig);

                    uow.Commit();
                }

                db.Aop.AuditValue -= SyncDataAuditValue;

                Console.WriteLine("sync data succeed\r\n");
            }
            catch (Exception ex)
            {
                throw new Exception($"sync data failed.\n{ex.Message}\r\n");
            }
        }

        /// <summary>
        /// 生成极简数据
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task GenerateSimpleJsonData(IFreeSql db)
        {
            try
            {
                Console.WriteLine("\r\ngenerate data started");

                #region 数据表

                #region 数据字典
                var dictionaries = await db.Queryable<DictionaryEntity>().ToListAsync(a => new
                {
                    a.Id,
                    a.ParentId,
                    a.Name,
                    a.Code,
                    a.Value,
                    a.Description,
                    a.Sort
                });
                #endregion

                #region 接口
                var apis = await db.Queryable<ApiEntity>().ToListAsync(a => new
                {
                    a.Id,
                    a.ParentId,
                    a.Name,
                    a.Label,
                    a.Path,
                    a.HttpMethods,
                    a.Description,
                    a.Sort
                });
                #endregion

                #region 视图
                var views = await db.Queryable<ViewEntity>().ToListAsync(a => new
                {
                    a.Id,
                    a.ParentId,
                    a.Name,
                    a.Label,
                    a.Path,
                    a.Description,
                    a.Sort
                });
                #endregion

                #region 权限
                var permissions = await db.Queryable<PermissionEntity>().ToListAsync(a => new
                {
                    a.Id,
                    a.ParentId,
                    a.Label,
                    a.Code,
                    a.Type,
                    a.ViewId,
                    a.ApiId,
                    a.Path,
                    a.Icon,
                    a.Closable,
                    a.Opened,
                    a.NewWindow,
                    a.External,
                    a.Sort,
                    a.Description
                });
                #endregion

                #region 用户
                var users = await db.Queryable<UserEntity>().ToListAsync(a => new
                {
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
                    a.Id,
                    a.Name,
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

                #endregion

                if(!(users?.Count > 0))
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
                    dictionaries,
                    apis,
                    views,
                    permissions,
                    users,
                    roles,
                    userRoles,
                    rolePermissions
                },
                //Formatting.Indented, 
                settings
                );
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Db/Data/data.json").ToPath();
                FileHelper.WriteFile(filePath, jsonData);
                #endregion

                Console.WriteLine("generate data succeed\r\n");
            }
            catch (Exception ex)
            {
                throw new Exception($"generate data failed。\n{ex.Message}\r\n");
            }
        }
    }
}
