using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.DictionaryType.Input;
using System.Threading.Tasks;

namespace My.Admin.Service.Admin.DictionaryType
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