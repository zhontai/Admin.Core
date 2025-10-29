using System.Threading.Tasks;
using ZhonTai.Admin.Core;
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

public class CustomGenerateData : GenerateData, IGenerateData
{
    /// <summary>
    /// 生成数据到InitData/Dev 手动新建Dev目录
    /// </summary>
    /// <param name="db"></param>
    /// <param name="appConfig"></param>
    /// <returns></returns>
    public virtual async Task GenerateDataAsync(IFreeSql db, AppConfig appConfig)
    {
        var outPath = AppInfo.GetOptions<DbConfig>().SyncDataPath;
        await SaveEntityAsync<DevGroupEntity>(db, appConfig, outPath);
        await SaveEntityAsync<DevTemplateEntity>(db, appConfig, outPath);
        await SaveEntityAsync<DevProjectEntity>(db, appConfig, outPath);
        await SaveEntityAsync<DevProjectModelEntity>(db, appConfig, outPath);
        await SaveEntityAsync<DevProjectModelFieldEntity>(db, appConfig, outPath);
        await SaveEntityAsync<DevProjectGenEntity>(db, appConfig, outPath);
        await SaveEntityAsync<CodeGenEntity>(db, appConfig, outPath);
    }
}
