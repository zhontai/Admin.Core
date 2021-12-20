using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Personnel;
using Admin.Core.Repository.Personnel;
using Admin.Core.Service.Personnel.Organization.Input;
using Admin.Core.Service.Personnel.Organization.Output;
using System.Threading.Tasks;

namespace Admin.Core.Service.Personnel.Organization
{
    public class OrganizationService : BaseService, IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var result = await _organizationRepository.GetAsync<OrganizationGetOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> GetListAsync(string key)
        {
            var data = await _organizationRepository
                .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync<OrganizationListOutput>();

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> AddAsync(OrganizationAddInput input)
        {
            var dictionary = Mapper.Map<OrganizationEntity>(input);
            var id = (await _organizationRepository.InsertAsync(dictionary)).Id;
            return ResponseOutput.Result(id > 0);
        }

        public async Task<IResponseOutput> UpdateAsync(OrganizationUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _organizationRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("数据字典不存在！");
            }

            Mapper.Map(input, entity);
            await _organizationRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var result = await _organizationRepository.DeleteRecursiveAsync(a => a.Id == id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(long id)
        {
            var result = await _organizationRepository.SoftDeleteRecursiveAsync(a => a.Id == id);

            return ResponseOutput.Result(result);
        }
    }
}