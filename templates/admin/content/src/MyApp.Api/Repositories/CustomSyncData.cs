using System.Threading.Tasks;
using ZhonTai.Admin.Core.Configs;
using MyApp.Api.Domain.Module;
using ZhonTai.Admin.Core.Db.Data;
using FreeSql;
using System.Linq;
using System;

namespace ZhonTai.Admin.Repositories;

public class CustomSyncData : SyncData, ISyncData
{
    /// <summary>
    /// 初始化模块
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    private async Task InitModuleAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        var tableName = GetTableName<ModuleEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<ModuleEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var dataList = GetData<ModuleEntity>(path: "InitData/App");

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var ids = dataList.Select(e => e.Id).ToList();
            var recordList = await rep.Where(a => ids.Contains(a.Id)).ToListAsync();

            //新增
            var recordIds = recordList.Select(a => a.Id).ToList();
            var insertDataList = dataList.Where(a => !recordIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (dbConfig.SysUpdateData && recordList?.Count > 0)
            {
                var updateDataList = dataList.Where(a => recordIds.Contains(a.Id));
                await rep.UpdateAsync(updateDataList);
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

    public virtual async Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null)
    {
        using var unitOfWork = db.CreateUnitOfWork();

        try
        {
            var isTenant = appConfig.Tenant;

            await InitModuleAsync(db, unitOfWork, dbConfig);

            unitOfWork.Commit();
        }
        catch (Exception)
        {
            unitOfWork.Rollback();
            throw;
        }
    }
}
