using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Entities;
using FreeSql;
using Mapster;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Admin.Core.Db.Data;

public abstract class SyncData
{
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
    /// 获得表名
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected static string GetTableName<T>() where T : class, new()
    {
        var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
        return table.Name;
    }

    protected static bool IsSyncData(string tableName, DbConfig dbConfig)
    {
        var isSyncData = true;

        var hasDataIncludeTables = dbConfig.SyncDataIncludeTables?.Length > 0;
        if (hasDataIncludeTables && !dbConfig.SyncDataIncludeTables.Contains(tableName))
        {
            isSyncData = false;
        }

        var hasSyncDataExcludeTables = dbConfig.SyncDataExcludeTables?.Length > 0;
        if (hasSyncDataExcludeTables && dbConfig.SyncDataExcludeTables.Contains(tableName))
        {
            isSyncData = false;
        }

        return isSyncData;
    }

    /// <summary>
    /// 初始化数据表数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="tran"></param>
    /// <param name="dataList"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    protected virtual async Task InitDataAsync<T>(
        IFreeSql db,
        System.Data.Common.DbTransaction tran,
        T[] dataList,
        DbConfig dbConfig = null
    ) where T : class, new()
    {
        var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
        var tableName = table.Name;

        try
        {
            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            var insertOrUpdate = db.InsertOrUpdate<T>();
            if (tran != null)
            {
                insertOrUpdate = insertOrUpdate.WithTransaction(tran);
            }
            if (!dbConfig.SysUpdateData)
            {
                insertOrUpdate.IfExistsDoNothing();
            }
            await insertOrUpdate.SetSource(dataList).ExecuteAffrowsAsync();

            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            var msg = $"table: {tableName} sync data failed.\n{ex.Message}";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }
    }

    protected virtual T[] GetData<T>(bool isTenant = false, string path = "InitData/Admin")
    {
        var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
        var fileName = $"{table.Name}{(isTenant ? ".tenant" : "")}.json";
        var filePath = Path.Combine(AppContext.BaseDirectory, $"{path}/{fileName}").ToPath();
        if (!File.Exists(filePath))
        {
            var msg = $"数据文件{filePath}不存在";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }
        var jsonData = FileHelper.ReadFile(filePath);
        var data = JsonConvert.DeserializeObject<T[]>(jsonData);

        return data;
    }

    /// <summary>
    /// 同步实体数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig">模块数据库配置</param>
    /// <param name="appConfig">应用配置</param>
    /// <param name="readPath">读取数据路径 InitData/xxx </param>
    /// <param name="processChilds">处理子级列表</param>
    /// <returns></returns>
    protected virtual async Task SyncEntityAsync<T>(IFreeSql db, 
        IRepositoryUnitOfWork unitOfWork, 
        DbConfig dbConfig, 
        AppConfig appConfig, 
        string readPath = null, 
        bool processChilds = false) 
        where T : Entity<long>, new()
    {
        if (processChilds && !typeof(T).IsAssignableTo(typeof(IChilds<T>)))
        {
            throw new InvalidOperationException("processChilds is true but T does not implement IChilds<T>");
        }

        var tableName = GetTableName<T>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var isTenant = appConfig.Tenant && typeof(T).IsAssignableTo(typeof(EntityTenant));
            var rep = db.GetRepository<T>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var dataList = GetData<T>(isTenant, readPath ?? dbConfig.SyncDataPath);

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            if (processChilds)
            {
                dataList = dataList.ToList().ToPlainList((a) => ((IChilds<T>)a).Childs).ToArray();
            }

            //查询
            var dataIds = dataList.Select(e => e.Id).ToList();
            var dbDataList = await rep.Where(a => dataIds.Contains(a.Id)).ToListAsync();

            //新增
            var dbDataIds = dbDataList.Select(a => a.Id).ToList();
            var insertDataList = dataList.Where(a => !dbDataIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (dbConfig.SysUpdateData && dbDataList?.Count > 0)
            {
                foreach (var dbData in dbDataList)
                {
                    var data = dataList.Where(a => a.Id == dbData.Id).First();
                    data.Adapt(dbData);
                }

                await rep.UpdateAsync(dbDataList);
            }

            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            var msg = $"table: {tableName} sync data failed.\n{ex.Message}";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }
    }
}
