using ZhonTai.Admin.Core.Dto;
using System.Threading.Tasks;
using MyApp.Api.Domain.Module.Dto;
using MyApp.Api.Services.Module.Input;
using MyApp.Api.Services.Module.Output;

namespace MyApp.Api.Services.Module;

/// <summary>
/// 模块接口
/// </summary>
public interface IModuleService
{
    Task<ModuleGetOutput> GetAsync(long id);

    Task<PageOutput<ModuleListOutput>> GetPageAsync(PageInput<ModuleGetPageDto> input);

    Task<long> AddAsync(ModuleAddInput input);

    Task UpdateAsync(ModuleUpdateInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}