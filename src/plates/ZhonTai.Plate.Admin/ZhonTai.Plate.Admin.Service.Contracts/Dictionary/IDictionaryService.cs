using ZhonTai.Common.Input;
using ZhonTai.Common.Output;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.Domain.Dictionary;
using ZhonTai.Plate.Admin.Service.Dictionary.Input;

namespace ZhonTai.Plate.Admin.Service.Dictionary
{
    public partial interface IDictionaryService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> PageAsync(PageInput<DictionaryEntity> model);

        Task<IResponseOutput> AddAsync(DictionaryAddInput input);

        Task<IResponseOutput> UpdateAsync(DictionaryUpdateInput input);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<IResponseOutput> SoftDeleteAsync(long id);

        Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids);
    }
}