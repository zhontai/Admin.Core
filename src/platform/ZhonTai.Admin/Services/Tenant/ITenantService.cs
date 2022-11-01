using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Tenant.Dto;
using ZhonTai.Admin.Services.Tenant.Dto;

namespace ZhonTai.Admin.Services.Tenant;

/// <summary>
/// 租户接口
/// </summary>
public interface ITenantService
{
    Task<TenantGetOutput> GetAsync(long id);

    Task<PageOutput<TenantListOutput>> GetPageAsync(PageInput<TenantGetPageDto> input);

    Task<long> AddAsync(TenantAddInput input);

    Task UpdateAsync(TenantUpdateInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}