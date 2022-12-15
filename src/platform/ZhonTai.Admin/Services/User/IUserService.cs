using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    Task<UserGetOutput> GetAsync(long id);

    Task<PageOutput<UserGetPageOutput>> GetPageAsync(PageInput<UserGetPageDto> input);

    Task<AuthLoginOutput> GetLoginUserAsync(long id);

    Task<DataPermissionDto> GetDataPermissionAsync();

    Task<long> AddAsync(UserAddInput input);

    Task<long> AddMemberAsync(UserAddMemberInput input);

    Task UpdateAsync(UserUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);

    Task ChangePasswordAsync(UserChangePasswordInput input);

    Task<string> ResetPasswordAsync(UserResetPasswordInput input);

    Task SetManagerAsync(UserSetManagerInput input);

    Task UpdateBasicAsync(UserUpdateBasicInput input);

    Task<UserGetBasicOutput> GetBasicAsync();

    Task<IList<UserPermissionsOutput>> GetPermissionsAsync();

    Task<string> AvatarUpload([FromForm] IFormFile file, bool autoUpdate = false);
}