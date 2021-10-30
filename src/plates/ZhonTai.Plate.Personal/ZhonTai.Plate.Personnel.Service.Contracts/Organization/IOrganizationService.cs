using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Personnel.Service.Organization.Input;
using System.Threading.Tasks;

namespace ZhonTai.Plate.Personnel.Service.Organization
{
    public partial interface IOrganizationService
    {
        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetListAsync(string key);

        Task<IResultOutput> AddAsync(OrganizationAddInput input);

        Task<IResultOutput> UpdateAsync(OrganizationUpdateInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> SoftDeleteAsync(long id);
    }
}