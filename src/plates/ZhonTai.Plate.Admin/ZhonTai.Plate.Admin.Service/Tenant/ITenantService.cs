using System.Threading.Tasks;
using ZhonTai.Common.Input;
using ZhonTai.Common.Output;
using ZhonTai.Plate.Admin.Domain.Tenant;
using ZhonTai.Plate.Admin.Service.Tenant.Input;

namespace ZhonTai.Plate.Admin.Service.Tenant
{
    public interface ITenantService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> PageAsync(PageInput<TenantEntity> input);

        Task<IResponseOutput> AddAsync(TenantAddInput input);

        Task<IResponseOutput> UpdateAsync(TenantUpdateInput input);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<IResponseOutput> SoftDeleteAsync(long id);

        Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids);
    }
}