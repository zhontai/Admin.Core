using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Personnel.Service.Organization.Input;
using System.Threading.Tasks;

namespace ZhonTai.Plate.Personnel.Service.Organization
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