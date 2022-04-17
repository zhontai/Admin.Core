using ZhonTai.Admin.Core.Dto;
using System.Threading.Tasks;
using ZhonTai.Admin.Services.DictionaryType.Dto;
using ZhonTai.Admin.Domain.DictionaryType.Dto;

namespace ZhonTai.Admin.Services.DictionaryType
{
    /// <summary>
    /// 数据字典类型接口
    /// </summary>
    public partial interface IDictionaryTypeService
    {
        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetPageAsync(PageInput<DictionaryTypeGetPageDto> input);

        Task<IResultOutput> AddAsync(DictionaryTypeAddInput input);

        Task<IResultOutput> UpdateAsync(DictionaryTypeUpdateInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> SoftDeleteAsync(long id);

        Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);
    }
}