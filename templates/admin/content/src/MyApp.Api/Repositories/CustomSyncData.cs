using System.Threading.Tasks;
using ZhonTai.Admin.Core.Configs;
using MyApp.Api.Domain.Module;
using ZhonTai.Admin.Core.Db.Data;
using System;

namespace ZhonTai.Admin.Repositories;

public class CustomSyncData : SyncData, ISyncData
{
    public virtual async Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null)
    {
        using var unitOfWork = db.CreateUnitOfWork();

        try
        {
            var isTenant = appConfig.Tenant;

            await SyncEntityAsync<ModuleEntity>(db, unitOfWork, dbConfig, appConfig);

            unitOfWork.Commit();
        }
        catch (Exception)
        {
            unitOfWork.Rollback();
            throw;
        }
    }
}
