using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.Organization.Input;
using System.Threading.Tasks;

namespace ZhonTai.Admin.Services.Organization
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