using ZhonTai.Admin.Core.Dto;
using System.Threading.Tasks;
using ZhonTai.Admin.Services.Dictionary.Dto;
using ZhonTai.Admin.Domain.Dictionary.Dto;

namespace ZhonTai.Admin.Services.Dictionary;

/// <summary>
/// 数据字典接口
/// </summary>
public partial interface IDictionaryService
{
    Task<DictionaryGetOutput> GetAsync(long id);

    Task<PageOutput<DictionaryListOutput>> GetPageAsync(PageInput<DictionaryGetPageDto> input);

    Task<long> AddAsync(DictionaryAddInput input);

    Task UpdateAsync(DictionaryUpdateInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}