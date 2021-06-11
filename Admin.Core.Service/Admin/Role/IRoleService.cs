using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Role.Input;
using System.Threading.Tasks;

namespace Admin.Core.Service.Admin.Role
{
    public interface IRoleService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> PageAsync(PageInput<RoleEntity> input);

        Task<IResponseOutput> AddAsync(RoleAddInput input);

        Task<IResponseOutput> UpdateAsync(RoleUpdateInput input);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<IResponseOutput> SoftDeleteAsync(long id);

        Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids);
    }
}