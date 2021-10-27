using ZhonTai.Common.Input;
using ZhonTai.Common.Output;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.Domain.DictionaryType;
using ZhonTai.Plate.Admin.Service.DictionaryType.Input;

namespace ZhonTai.Plate.Admin.Service.DictionaryType
{
    public partial interface IDictionaryTypeService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> PageAsync(PageInput<DictionaryTypeEntity> model);

        Task<IResponseOutput> AddAsync(DictionaryTypeAddInput input);

        Task<IResponseOutput> UpdateAsync(DictionaryTypeUpdateInput input);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<IResponseOutput> SoftDeleteAsync(long id);

        Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids);
    }
}