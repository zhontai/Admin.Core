using System.Threading.Tasks;
using AutoMapper;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.Tenant.Input;
using Admin.Core.Service.Admin.Tenant.Output;

namespace Admin.Core.Service.Admin.Tenant
{	
	public class TenantService : ITenantService
    {
        private readonly IUser _user;
        private readonly IMapper _mapper;
        private readonly ITenantRepository _tenantRepository;
        public TenantService(
            IUser user,
            IMapper mapper,
            ITenantRepository tenantRepository
        )
        {
            _user = user;
            _mapper = mapper;
            _tenantRepository = tenantRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var result = await _tenantRepository.GetAsync<TenantGetOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> PageAsync(PageInput<TenantEntity> input)
        {
            var key = input.Filter?.Name;

            var list = await _tenantRepository.Select
            .WhereIf(key.NotNull(), a => a.Name.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<TenantListOutput>();

            var data = new PageOutput<TenantListOutput>()
            {
                List = list,
                Total = total
            };
            
            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> AddAsync(TenantAddInput input)
        {
            var entity = _mapper.Map<TenantEntity>(input);
            var id = (await _tenantRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id > 0);
        }

        public async Task<IResponseOutput> UpdateAsync(TenantUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _tenantRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("×â»§²»´æÔÚ£¡");
            }

            _mapper.Map(input, entity);
            await _tenantRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _tenantRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(long id)
        {
            var result = await _tenantRepository.SoftDeleteAsync(id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids)
        {
            var result = await _tenantRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
    }
}
