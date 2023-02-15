using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using FreeSql;
using FreeSql.DataAnnotations;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;

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
    /// 初始化数据表数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="tran"></param>
    /// <param name="dataList"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    protected virtual async Task InitDataAsync<T>(
        IFreeSql db,
        IUnitOfWork unitOfWork,
        System.Data.Common.DbTransaction tran,
        T[] dataList,
        DbConfig dbConfig = null
    ) where T : class, new()
    {
        var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
        var tableName = table.Name;

        try
        {
            //if (await db.Queryable<T>().DisableGlobalFilter(FilterNames.Tenant, FilterNames.Member).AnyAsync())
            //{
            //    Console.WriteLine($" table: {tableName} record already exists");
            //    return;
            //}

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($" table: {tableName} import data []");
                return;
            }

            var insertOrUpdate = db.InsertOrUpdate<T>();
            if (unitOfWork != null)
            {
                insertOrUpdate = insertOrUpdate.WithTransaction(tran);
            }
            if (!dbConfig.SysUpdateData)
            {
                insertOrUpdate.IfExistsDoNothing();
            }
            await insertOrUpdate.SetSource(dataList).ExecuteAffrowsAsync();

            Console.WriteLine($" table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

    protected virtual T[] GetData<T>(bool isTenant = false, string path = "InitData/Admin")
    {
        var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
        var fileName = $"{table.Name}{(isTenant ? ".tenant" : "")}.json";
        var filePath = Path.Combine(AppContext.BaseDirectory, $"{path}/{fileName}").ToPath();
        if (!File.Exists(filePath))
        {
            var msg = $"文件{filePath}不存在";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }
        var jsonData = FileHelper.ReadFile(filePath);
        var data = JsonConvert.DeserializeObject<T[]>(jsonData);

        return data;
    }
}
