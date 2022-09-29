using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.Org.Input;
using System.Threading.Tasks;

namespace ZhonTai.Admin.Services.Org;

public partial interface IOrgService
{
    Task<IResultOutput> GetAsync(long id);

    Task<IResultOutput> GetListAsync(string key);

    Task<IResultOutput> AddAsync(OrgAddInput input);

    Task<IResultOutput> UpdateAsync(OrgUpdateInput input);

    Task<IResultOutput> DeleteAsync(long id);

    Task<IResultOutput> SoftDeleteAsync(long id);
}