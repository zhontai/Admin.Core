using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Personnel;
using Admin.Core.Service.Personnel.Organization.Input;
using System.Threading.Tasks;

namespace Admin.Core.Service.Personnel.Organization
{
    public partial interface IOrganizationService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> GetListAsync(string key);

        Task<IResponseOutput> AddAsync(OrganizationAddInput input);

        Task<IResponseOutput> UpdateAsync(OrganizationUpdateInput input);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<IResponseOutput> SoftDeleteAsync(long id);
    }
}