using System.Threading.Tasks;
using AutoMapper;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.Role.Input;
using Admin.Core.Service.Admin.Role.Output;

namespace Admin.Core.Service.Admin.Role
{	
	public class RoleService : IRoleService
    {
        private readonly IUser _user;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        public RoleService(
            IUser user,
            IMapper mapper,
            IRoleRepository roleRepository
        )
        {
            _user = user;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var result = await _roleRepository.GetAsync<RoleGetOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> PageAsync(PageInput<RoleEntity> input)
        {
            var key = input.Filter?.Name;

            var list = await _roleRepository.Select
            .WhereIf(key.NotNull(), a => a.Name.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<RoleListOutput>();

            var data = new PageOutput<RoleListOutput>()
            {
                List = list,
                Total = total
            };
            
            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> AddAsync(RoleAddInput input)
        {
            var entity = _mapper.Map<RoleEntity>(input);
            var id = (await _roleRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id > 0);
        }

        public async Task<IResponseOutput> UpdateAsync(RoleUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _roleRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("½ÇÉ«²»´æÔÚ£¡");
            }

            _mapper.Map(input, entity);
            await _roleRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _roleRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(long id)
        {
            var result = await _roleRepository.SoftDeleteAsync(id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids)
        {
            var result = await _roleRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
    }
}
