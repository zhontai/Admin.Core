using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.Role.Input;
using System.Threading.Tasks;

namespace My.Admin.Service.Admin.Role
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