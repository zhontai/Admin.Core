using System.Threading.Tasks;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Db.Data;
using MyApp.Api.Contracts.Domain.Module;

namespace MyApp.Api.Repositories;

public class CustomGenerateData : GenerateData, IGenerateData
{
    public virtual async Task GenerateDataAsync(IFreeSql db, AppConfig appConfig)
    {
        var isTenant = appConfig.Tenant;

        var modules = await db.Queryable<ModuleEntity>().ToListAsync();

        SaveDataToJsonFile<ModuleEntity>(modules, isTenant, path: "InitData/App");
    }
}
