using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.Dictionary.Input;
using System.Threading.Tasks;

namespace My.Admin.Service.Admin.Dictionary
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