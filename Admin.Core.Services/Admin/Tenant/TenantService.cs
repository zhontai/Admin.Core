using System.Threading.Tasks;
using AutoMapper;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.Tenant.Input;
using Admin.Core.Service.Admin.Tenant.Output;
using Admin.Core.Common.Attributes;
using Admin.Core.Common.Helpers;

namespace Admin.Core.Service.Admin.Tenant
{
    public class TenantService : BaseService,ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public TenantService(
            ITenantRepository tenantRepository,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRolePermissionRepository rolePermissionRepository
        )
        {
            _tenantRepository = tenantRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionRepository = rolePermissionRepository;
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

        [Transaction]
        public async Task<IResponseOutput> AddAsync(TenantAddInput input)
        {
            var entity = Mapper.Map<TenantEntity>(input);
            var tenant = await _tenantRepository.InsertAsync(entity);

            var tenantId = tenant.Id;
            //添加角色
            var role = new RoleEntity { TenantId = tenantId, Code = "plat_admin", Name = "平台管理员", Enabled = true };
            await _roleRepository.InsertAsync(role);

            //添加用户
            var pwd = MD5Encrypt.Encrypt32("111111");
            var user = new UserEntity { TenantId = tenantId, UserName = input.Phone, NickName= input.RealName, Password = pwd, Status = 0 };
            await _userRepository.InsertAsync(user);

            //添加用户角色
            var userRole = new UserRoleEntity() { TenantId = tenantId, UserId = user.Id, RoleId = role.Id };
            await _userRoleRepository.InsertAsync(userRole);

            //更新租户用户
            tenant.UserId = user.Id;
            await _tenantRepository.UpdateAsync(tenant);

            return ResponseOutput.Ok();
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
                return ResponseOutput.NotOk("租户不存在！");
            }

            Mapper.Map(input, entity);
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
