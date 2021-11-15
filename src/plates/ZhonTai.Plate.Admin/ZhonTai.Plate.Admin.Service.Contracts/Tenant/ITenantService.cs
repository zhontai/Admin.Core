using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain.Tenant.Dto;
using ZhonTai.Plate.Admin.Service.Tenant.Dto;

namespace ZhonTai.Plate.Admin.Service.Tenant
{
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