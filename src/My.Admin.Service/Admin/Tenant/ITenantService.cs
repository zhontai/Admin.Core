using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.Tenant.Input;
using System.Threading.Tasks;

namespace My.Admin.Service.Admin.Tenant
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