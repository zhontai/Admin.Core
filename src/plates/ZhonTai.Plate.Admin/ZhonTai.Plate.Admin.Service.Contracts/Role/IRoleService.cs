using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain.Role.Dto;
using ZhonTai.Plate.Admin.Service.Role.Input;

namespace ZhonTai.Plate.Admin.Service.Role
{
    public interface IRoleService
    {
        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetPageAsync(PageInput<RoleGetPageDto> input);

        Task<IResultOutput> AddAsync(RoleAddInput input);

        Task<IResultOutput> UpdateAsync(RoleUpdateInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> SoftDeleteAsync(long id);

        Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);
    }
}