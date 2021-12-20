using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.DictionaryType.Input;
using System.Threading.Tasks;

namespace Admin.Core.Service.Admin.DictionaryType
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