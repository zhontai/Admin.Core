using System.Collections.Generic;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain.User;
using ZhonTai.Plate.Admin.Service.Auth.Output;
using ZhonTai.Plate.Admin.Service.User.Input;
using ZhonTai.Plate.Admin.Service.User.Output;

namespace ZhonTai.Plate.Admin.Service.User
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public interface IUserService
    {
        Task<ResponseOutput<AuthLoginOutput>> GetLoginUserAsync(long id);

        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> GetSelectAsync();

        Task<IResponseOutput> PageAsync(PageInput<UserEntity> input);

        Task<IResponseOutput> AddAsync(UserAddInput input);

        Task<IResponseOutput> UpdateAsync(UserUpdateInput input);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<IResponseOutput> SoftDeleteAsync(long id);

        Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids);

        Task<IResponseOutput> ChangePasswordAsync(UserChangePasswordInput input);

        Task<IResponseOutput> UpdateBasicAsync(UserUpdateBasicInput input);

        Task<IResponseOutput> GetBasicAsync();

        Task<IList<UserPermissionsOutput>> GetPermissionsAsync();
    }
}