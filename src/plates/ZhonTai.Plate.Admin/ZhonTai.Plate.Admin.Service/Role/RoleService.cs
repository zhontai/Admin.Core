using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Repositories;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain.Role;
using ZhonTai.Plate.Admin.Domain.RolePermission;
using ZhonTai.Plate.Admin.Service.Role.Input;
using ZhonTai.Plate.Admin.Service.Role.Output;
using ZhonTai.Plate.Admin.Domain.Role.Dto;

namespace ZhonTai.Plate.Admin.Service.Role
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRepositoryBase<RolePermissionEntity> _rolePermissionRepository;

        public RoleService(
            IRoleRepository roleRepository,
            IRepositoryBase<RolePermissionEntity> rolePermissionRepository
        )
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<IResultOutput> GetAsync(long id)
        {
            var result = await _roleRepository.GetAsync<RoleGetOutput>(id);
            return ResultOutput.Ok(result);
        }

        public async Task<IResultOutput> GetPageAsync(PageInput<RoleGetPageDto> input)
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

            return ResultOutput.Ok(data);
        }

        public async Task<IResultOutput> AddAsync(RoleAddInput input)
        {
            var entity = Mapper.Map<RoleEntity>(input);
            var id = (await _roleRepository.InsertAsync(entity)).Id;

            return ResultOutput.Result(id > 0);
        }

        public async Task<IResultOutput> UpdateAsync(RoleUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResultOutput.NotOk();
            }

            var entity = await _roleRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResultOutput.NotOk("½ÇÉ«²»´æÔÚ£¡");
            }

            Mapper.Map(input, entity);
            await _roleRepository.UpdateAsync(entity);
            return ResultOutput.Ok();
        }

        public async Task<IResultOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _roleRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResultOutput.Result(result);
        }

        public async Task<IResultOutput> SoftDeleteAsync(long id)
        {
            var result = await _roleRepository.SoftDeleteAsync(id);
            await _rolePermissionRepository.DeleteAsync(a => a.RoleId == id);

            return ResultOutput.Result(result);
        }

        public async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
        {
            var result = await _roleRepository.SoftDeleteAsync(ids);
            await _rolePermissionRepository.DeleteAsync(a => ids.Contains(a.RoleId));

            return ResultOutput.Result(result);
        }
    }
}