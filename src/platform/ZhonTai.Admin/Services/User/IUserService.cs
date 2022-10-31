using System.Collections.Generic;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.User.Dto;
using ZhonTai.Admin.Services.Auth.Dto;
using ZhonTai.Admin.Services.User.Dto;

namespace ZhonTai.Admin.Services.User;

/// <summary>
/// 用户接口
/// </summary>
public interface IUserService
{
    Task<IResultOutput> GetAsync(long id);

    Task<IResultOutput> GetPageAsync(PageInput<long?> input);

    Task<ResultOutput<AuthLoginOutput>> GetLoginUserAsync(long id);

    Task<DataPermissionDto> GetDataPermissionAsync();

    Task<IResultOutput> AddAsync(UserAddInput input);

    Task<IResultOutput> UpdateAsync(UserUpdateInput input);

    Task<IResultOutput> DeleteAsync(long id);

    Task<IResultOutput> BatchDeleteAsync(long[] ids);

    Task<IResultOutput> SoftDeleteAsync(long id);

    Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);

    Task<IResultOutput> ChangePasswordAsync(UserChangePasswordInput input);

    Task<IResultOutput> UpdateBasicAsync(UserUpdateBasicInput input);

    Task<IResultOutput> GetBasicAsync();

    Task<IList<UserPermissionsOutput>> GetPermissionsAsync();
}