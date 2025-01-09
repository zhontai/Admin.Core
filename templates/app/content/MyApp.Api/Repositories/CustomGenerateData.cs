using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Domain.Tenant;
using MyApp.Api.Domain.Module;
using ZhonTai.Admin.Core.Db.Data;

namespace ZhonTai.Admin.Repositories;

public class CustomGenerateData : GenerateData, IGenerateData
{
    public virtual async Task GenerateDataAsync(IFreeSql db, AppConfig appConfig)
    {
        var isTenant = appConfig.Tenant;

        var modules = await db.Queryable<ModuleEntity>().ToListAsync();

        if (isTenant)
        {
            var tenantIds = await db.Queryable<TenantEntity>().ToListAsync(a => a.Id);
            SaveDataToJsonFile<ModuleEntity>(modules.Where(a => tenantIds.Contains(a.TenantId.Value)));
        }

        SaveDataToJsonFile<ModuleEntity>(modules, isTenant, path: "InitData/App");
    }
}
