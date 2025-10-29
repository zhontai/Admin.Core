using System;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Db.Data;
using ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevGroup;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevProject;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevProjectGen;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevProjectModel;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevProjectModelField;
using ZhonTai.Module.Dev.Api.Contracts.Domain.DevTemplate;

namespace ZhonTai.Module.Dev.Repositories;

public class CustomSyncData : SyncData, ISyncData
{
    public virtual async Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null)
    {
        using var unitOfWork = db.CreateUnitOfWork();

        try
        {
            await SyncEntityAsync<DevGroupEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<DevTemplateEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<DevProjectEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<DevProjectModelEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<DevProjectModelFieldEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<DevProjectGenEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<CodeGenEntity>(db, unitOfWork, dbConfig, appConfig);

            unitOfWork.Commit();
        }
        catch (Exception)
        {
            unitOfWork.Rollback();
        }
    }
}
