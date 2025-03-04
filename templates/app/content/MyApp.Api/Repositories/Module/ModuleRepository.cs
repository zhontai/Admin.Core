using MyApp.Api.Contracts;
using MyApp.Api.Contracts.Domain.Module;
using MyApp.Api.Core.Repositories;
using ZhonTai.Admin.Core.Db.Transaction;

namespace MyApp.Api.Repositories.Module;

/// <summary>
/// Ä£¿é²Ö´¢
/// </summary>
public class ModuleRepository : AppRepositoryBase<ModuleEntity>, IModuleRepository
{
    public ModuleRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}