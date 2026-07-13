using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Contracts.Domain.Sso;
using ZhonTai.Admin.Contracts.Services.Sso;
using ZhonTai.Admin.Contracts.Services.Sso.Dto;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Resources;
using ZhonTai.Common.Helpers;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;

namespace ZhonTai.Admin.Services.Sso;

/// <summary>
/// 单点登录应用管理（第三方应用配置）
/// </summary>
[Order(95)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class SsoAppManageService : BaseService, ISsoAppManageService, IDynamicApi
{
    private readonly ISsoAppManageRepository _ssoAppRep;
    private readonly AdminLocalizer _adminLocalizer;

    public SsoAppManageService(ISsoAppManageRepository ssoAppRep, AdminLocalizer adminLocalizer)
    {
        _ssoAppRep = ssoAppRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 详情
    /// </summary>
    public async Task<SsoAppManageGetOutput> GetAsync(long id)
    {
        var output = await _ssoAppRep.Select.WhereDynamic(id).ToOneAsync<SsoAppManageGetOutput>();
        return output;
    }

    /// <summary>
    /// 分页列表
    /// </summary>
    [HttpPost]
    public async Task<PageOutput<SsoAppManageGetListOutput>> GetPageAsync(PageInput<SsoAppManageGetListInput> input)
    {
        var list = await _ssoAppRep.Select
        .WhereIf(input.Filter?.Name.NotNull() == true, a => a.AppName.Contains(input.Filter.Name))
        .OrderBy(a => a.Sort)
        .Count(out var total)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<SsoAppManageGetListOutput>();

        return new PageOutput<SsoAppManageGetListOutput> { List = list, Total = total };
    }

    /// <summary>
    /// 新增（密钥由系统自动生成）
    /// </summary>
    public async Task<long> AddAsync(SsoAppManageAddInput input)
    {
        if (await _ssoAppRep.Select.AnyAsync(a => a.AppId == input.AppId))
            throw ResultOutput.Exception(_adminLocalizer["应用Id已存在"]);

        var entity = Mapper.Map<SsoAppManageEntity>(input);
        entity.AppSecret = StringHelper.GenerateRandom(32);
        await _ssoAppRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    public async Task UpdateAsync(SsoAppManageUpdateInput input)
    {
        var entity = await _ssoAppRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception(_adminLocalizer["应用不存在"]);

        // 更新不允许修改 AppId（应用唯一标识）
        Mapper.Map(input, entity);
        await _ssoAppRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除（软删除）
    /// </summary>
    [AdminTransaction]
    public async Task SoftDeleteAsync(long id)
    {
        await _ssoAppRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 重置应用密钥，返回新密钥（旧密钥立即失效）
    /// </summary>
    public async Task<string> ResetSecretAsync(long id)
    {
        var entity = await _ssoAppRep.GetAsync(id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception(_adminLocalizer["应用不存在"]);

        entity.AppSecret = StringHelper.GenerateRandom(32);
        await _ssoAppRep.UpdateAsync(entity);

        return entity.AppSecret;
    }

    /// <summary>
    /// 单点登录按钮列表（仅返回启用的应用，脱敏，供前端动态渲染按钮，按 Sort 升序）
    /// </summary>
    [HttpPost]
    public async Task<List<SsoAppManageButtonOutput>> GetAppsAsync()
    {
        var list = await _ssoAppRep.Select
            .Where(a => a.Status == 1)
            .OrderBy(a => a.Sort)
            .ToListAsync<SsoAppManageButtonOutput>();

        return list;
    }
}
