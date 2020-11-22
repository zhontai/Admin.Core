
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Auth.Output;
using Admin.Core.Service.Admin.User.Input;
using Admin.Core.Service.Admin.User.Output;

namespace Admin.Core.Service.Admin.User
{
    /// <summary>
    /// 用户服务
    /// </summary>	
    public interface IUserService
	{
        Task<ResponseOutput<AuthLoginOutput>> GetLoginUserAsync(long id);

        Task<ResponseOutput<UserGetOutput>> GetAsync(long id);

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
