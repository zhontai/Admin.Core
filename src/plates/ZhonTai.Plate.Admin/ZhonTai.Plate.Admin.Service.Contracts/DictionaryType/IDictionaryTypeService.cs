using ZhonTai.Common.Domain.Dto;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.Service.DictionaryType.Input;
using ZhonTai.Plate.Admin.Domain.DictionaryType.Dto;

namespace ZhonTai.Plate.Admin.Service.DictionaryType
{
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