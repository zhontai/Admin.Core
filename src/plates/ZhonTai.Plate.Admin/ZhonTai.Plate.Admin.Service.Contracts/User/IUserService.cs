using System.Collections.Generic;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Service.Auth.Dto;
using ZhonTai.Plate.Admin.Service.User.Dto;

namespace ZhonTai.Plate.Admin.Service.User
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public interface IUserService
    {
        Task<ResultOutput<AuthLoginOutput>> GetLoginUserAsync(long id);

        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetSelectAsync();

        Task<IResultOutput> GetPageAsync(PageInput input);

        Task<IResultOutput> AddAsync(UserAddInput input);

        Task<IResultOutput> UpdateAsync(UserUpdateInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> SoftDeleteAsync(long id);

        Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);

        Task<IResultOutput> ChangePasswordAsync(UserChangePasswordInput input);

        Task<IResultOutput> UpdateBasicAsync(UserUpdateBasicInput input);

        Task<IResultOutput> GetBasicAsync();

        Task<IList<UserPermissionsOutput>> GetPermissionsAsync();
    }
}