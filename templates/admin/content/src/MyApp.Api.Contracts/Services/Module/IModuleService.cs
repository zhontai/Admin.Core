using ZhonTai.Admin.Core.Dto;
using System.Threading.Tasks;
using MyApp.Api.Contracts.Services.Module.Input;
using MyApp.Api.Contracts.Services.Module.Output;

namespace MyApp.Api.Contracts.Services.Module;

/// <summary>
/// 模块接口
/// </summary>
public interface IModuleService
{
    Task<ModuleGetOutput> GetAsync(long id);

    Task<PageOutput<ModuleGetPageOutput>> GetPageAsync(PageInput<ModuleGetPageInput> input);

    Task<long> AddAsync(ModuleAddInput input);

    Task UpdateAsync(ModuleUpdateInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}