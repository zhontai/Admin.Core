using ZhonTai.Admin.Core.Dto;
using System.Threading.Tasks;
using ZhonTai.Admin.Services.DictionaryType.Dto;
using ZhonTai.Admin.Domain.DictionaryType.Dto;

namespace ZhonTai.Admin.Services.DictionaryType;

/// <summary>
/// 数据字典类型接口
/// </summary>
public partial interface IDictionaryTypeService
{
    Task<DictionaryTypeGetOutput> GetAsync(long id);

    Task<PageOutput<DictionaryTypeListOutput>> GetPageAsync(PageInput<DictionaryTypeGetPageDto> input);

    Task<long> AddAsync(DictionaryTypeAddInput input);

    Task UpdateAsync(DictionaryTypeUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}