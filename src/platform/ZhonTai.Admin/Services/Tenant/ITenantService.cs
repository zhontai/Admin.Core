using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Tenant.Dto;
using ZhonTai.Admin.Services.Tenant.Dto;

namespace ZhonTai.Admin.Services.Tenant
{
    /// <summary>
    /// ×â»§½Ó¿Ú
    /// </summary>
    public interface ITenantService
    {
        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetPageAsync(PageInput<TenantGetPageDto> input);

        Task<IResultOutput> AddAsync(TenantAddInput input);

        Task<IResultOutput> UpdateAsync(TenantUpdateInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> SoftDeleteAsync(long id);

        Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);
    }
}