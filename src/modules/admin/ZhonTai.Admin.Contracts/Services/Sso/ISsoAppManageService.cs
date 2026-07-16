using ZhonTai.Admin.Contracts.Services.Sso.Dto;
using ZhonTai.Admin.Core.Dto;

namespace ZhonTai.Admin.Contracts.Services.Sso;

/// <summary>
/// 单点登录应用管理接口
/// </summary>
public interface ISsoAppManageService
{
    Task<SsoAppManageGetOutput> GetAsync(long id);

    Task<PageOutput<SsoAppManageGetListOutput>> GetPageAsync(PageInput<SsoAppManageGetListInput> input);

    Task<long> AddAsync(SsoAppManageAddInput input);

    Task UpdateAsync(SsoAppManageUpdateInput input);

    Task SoftDeleteAsync(long id);

    /// <summary>
    /// 重置应用密钥
    /// </summary>
    Task<string> ResetSecretAsync(long id);

    /// <summary>
    /// 单点登录按钮列表（启用的应用，脱敏，按 Sort 升序）
    /// </summary>
    Task<List<SsoAppManageButtonOutput>> GetAppsAsync();
}
