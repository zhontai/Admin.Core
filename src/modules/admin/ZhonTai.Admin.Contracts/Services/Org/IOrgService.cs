using ZhonTai.Admin.Services.Org.Input;
using ZhonTai.Admin.Services.Org.Output;

namespace ZhonTai.Admin.Services.Org;

/// <summary>
/// 部门接口
/// </summary>
public partial interface IOrgService
{
    Task<OrgGetOutput> GetAsync(long id);

    Task<List<OrgGetListOutput>> GetListAsync(string key);

    Task<long> AddAsync(OrgAddInput input);

    Task UpdateAsync(OrgUpdateInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);
}