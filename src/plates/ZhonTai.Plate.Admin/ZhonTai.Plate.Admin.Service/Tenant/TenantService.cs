using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Common.Attributes;
using ZhonTai.Common.Domain.Repositories;
using ZhonTai.Common.Helpers;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain.Role;
using ZhonTai.Plate.Admin.Domain.RolePermission;
using ZhonTai.Plate.Admin.Domain.Tenant;
using ZhonTai.Plate.Admin.Domain.User;
using ZhonTai.Plate.Admin.Domain.UserRole;
using ZhonTai.Plate.Admin.Service.Tenant.Input;
using ZhonTai.Plate.Admin.Service.Tenant.Output;

namespace ZhonTai.Plate.Admin.Service.Tenant
{
    public class TenantService : BaseService, ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepositoryBase<UserRoleEntity> _userRoleRepository;
        private readonly IRepositoryBase<RolePermissionEntity> _rolePermissionRepository;

        public TenantService(
            ITenantRepository tenantRepository,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IRepositoryBase<UserRoleEntity> userRoleRepository,
            IRepositoryBase<RolePermissionEntity> rolePermissionRepository
        )
        {
            _tenantRepository = tenantRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<IResultOutput> GetAsync(long id)
        {
            var result = await _tenantRepository.GetAsync<TenantGetOutput>(id);
            return ResultOutput.Ok(result);
        }

        public async Task<IResultOutput> GetPageAsync(PageInput<TenantEntity> input)
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

            return ResultOutput.Ok(data);
        }

        [Transaction]
        public async Task<IResultOutput> AddAsync(TenantAddInput input)
        {
            var entity = Mapper.Map<TenantEntity>(input);
            var tenant = await _tenantRepository.InsertAsync(entity);

            var tenantId = tenant.Id;

            //添加用户
            var pwd = MD5Encrypt.Encrypt32("111111");
            var user = new UserEntity { TenantId = tenantId, UserName = input.Phone, NickName = input.RealName, Password = pwd, Status = 0 };
            await _userRepository.InsertAsync(user);

            //添加角色
            var role = new RoleEntity { TenantId = tenantId, Code = "plat_admin", Name = "平台管理员", Enabled = true };
            await _roleRepository.InsertAsync(role);

            //添加用户角色
            var userRole = new UserRoleEntity() { UserId = user.Id, RoleId = role.Id };
            await _userRoleRepository.InsertAsync(userRole);

            //更新租户用户和角色
            tenant.UserId = user.Id;
            tenant.RoleId = role.Id;
            await _tenantRepository.UpdateAsync(tenant);

            return ResultOutput.Ok();
        }

        public async Task<IResultOutput> UpdateAsync(TenantUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResultOutput.NotOk();
            }

            var entity = await _tenantRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResultOutput.NotOk("租户不存在！");
            }

            Mapper.Map(input, entity);
            await _tenantRepository.UpdateAsync(entity);
            return ResultOutput.Ok();
        }

        [Transaction]
        public async Task<IResultOutput> DeleteAsync(long id)
        {
            //删除角色权限
            await _rolePermissionRepository.Where(a => a.Role.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

            //删除用户角色
            await _userRoleRepository.Where(a => a.User.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

            //删除用户
            await _userRepository.Where(a => a.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

            //删除角色
            await _roleRepository.Where(a => a.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

            //删除租户
            await _tenantRepository.DeleteAsync(id);

            return ResultOutput.Ok();
        }

        [Transaction]
        public async Task<IResultOutput> SoftDeleteAsync(long id)
        {
            //删除用户
            await _userRepository.SoftDeleteAsync(a => a.TenantId == id, "Tenant");

            //删除角色
            await _roleRepository.SoftDeleteAsync(a => a.TenantId == id, "Tenant");

            //删除租户
            var result = await _tenantRepository.SoftDeleteAsync(id);

            return ResultOutput.Result(result);
        }

        [Transaction]
        public async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
        {
            //删除用户
            await _userRepository.SoftDeleteAsync(a => ids.Contains(a.TenantId.Value), "Tenant");

            //删除角色
            await _roleRepository.SoftDeleteAsync(a => ids.Contains(a.TenantId.Value), "Tenant");

            //删除租户
            var result = await _tenantRepository.SoftDeleteAsync(ids);

            return ResultOutput.Result(result);
        }
    }
}