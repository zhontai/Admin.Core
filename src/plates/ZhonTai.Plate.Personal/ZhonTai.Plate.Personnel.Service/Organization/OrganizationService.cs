using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Personnel.Service.Organization.Input;
using ZhonTai.Plate.Personnel.Service.Organization.Output;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.Service;
using ZhonTai.Plate.Personnel.Domain.Organization;

namespace ZhonTai.Plate.Personnel.Service.Organization
{
    public class OrganizationService : BaseService, IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<IResultOutput> GetAsync(long id)
        {
            var result = await _organizationRepository.GetAsync<OrganizationGetOutput>(id);
            return ResultOutput.Ok(result);
        }

        public async Task<IResultOutput> GetListAsync(string key)
        {
            var data = await _organizationRepository
                .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync<OrganizationListOutput>();

            return ResultOutput.Ok(data);
        }

        public async Task<IResultOutput> AddAsync(OrganizationAddInput input)
        {
            var dictionary = Mapper.Map<OrganizationEntity>(input);
            var id = (await _organizationRepository.InsertAsync(dictionary)).Id;
            return ResultOutput.Result(id > 0);
        }

        public async Task<IResultOutput> UpdateAsync(OrganizationUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResultOutput.NotOk();
            }

            var entity = await _organizationRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResultOutput.NotOk("数据字典不存在！");
            }

            Mapper.Map(input, entity);
            await _organizationRepository.UpdateAsync(entity);
            return ResultOutput.Ok();
        }

        public async Task<IResultOutput> DeleteAsync(long id)
        {
            var result = await _organizationRepository.DeleteRecursiveAsync(a => a.Id == id);

            return ResultOutput.Result(result);
        }

        public async Task<IResultOutput> SoftDeleteAsync(long id)
        {
            var result = await _organizationRepository.SoftDeleteRecursiveAsync(a => a.Id == id);

            return ResultOutput.Result(result);
        }
    }
}