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
    Task<IResultOutput> GetAsync(long id);

    Task<IResultOutput> GetPageAsync(PageInput<DictionaryGetPageDto> input);

    Task<IResultOutput> AddAsync(DictionaryAddInput input);

    Task<IResultOutput> UpdateAsync(DictionaryUpdateInput input);

    Task<IResultOutput> DeleteAsync(long id);

    Task<IResultOutput> SoftDeleteAsync(long id);

    Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);
}