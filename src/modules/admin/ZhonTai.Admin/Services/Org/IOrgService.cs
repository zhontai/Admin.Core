using ZhonTai.Admin.Services.Org.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using ZhonTai.Admin.Services.Org.Output;

namespace ZhonTai.Admin.Services.Org;

public partial interface IOrgService
{
    Task<OrgGetOutput> GetAsync(long id);

    Task<List<OrgListOutput>> GetListAsync(string key);

    Task<long> AddAsync(OrgAddInput input);

    Task UpdateAsync(OrgUpdateInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);
}