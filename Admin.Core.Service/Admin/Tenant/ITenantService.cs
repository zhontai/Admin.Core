using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Tenant.Input;

namespace Admin.Core.Service.Admin.Tenant
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
