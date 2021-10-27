using System.Threading.Tasks;
using ZhonTai.Common.Input;
using ZhonTai.Common.Output;
using ZhonTai.Plate.Admin.Domain.Role;
using ZhonTai.Plate.Admin.Service.Role.Input;

namespace ZhonTai.Plate.Admin.Service.Role
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