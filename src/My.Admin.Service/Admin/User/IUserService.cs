using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.Auth.Output;
using My.Admin.Service.Admin.User.Input;
using My.Admin.Service.Admin.User.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Admin.Service.Admin.User
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