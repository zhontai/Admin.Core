using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Personnel;
using My.Admin.Service.Personnel.Organization.Input;
using System.Threading.Tasks;

namespace My.Admin.Service.Personnel.Organization
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