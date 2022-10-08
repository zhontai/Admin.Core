using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Services.Tenant.Dto;
using ZhonTai.Admin.Domain.Tenant.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Domain.Staff;
using ZhonTai.Admin.Domain;

namespace ZhonTai.Admin.Services.Tenant;

/// <summary>
/// 租户服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class TenantService : BaseService, ITenantService, IDynamicApi
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepositoryBase<UserRoleEntity> _userRoleRepository;
    private readonly IRepositoryBase<RolePermissionEntity> _rolePermissionRepository;
    private IOrgRepository _orgRepository => LazyGetRequiredService<IOrgRepository>();
    private IStaffRepository _staffRepository => LazyGetRequiredService<IStaffRepository>();
    private IRepositoryBase<UserOrgEntity> _userOrgRepository => LazyGetRequiredService<IRepositoryBase<UserOrgEntity>>();
    private AppConfig _appConfig => LazyGetRequiredService<AppConfig>();

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

    /// <summary>
    /// 查询租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetAsync(long id)
    {
        var result = await _tenantRepository.GetAsync<TenantGetOutput>(id);
        return ResultOutput.Ok(result);
    }

    /// <summary>
    /// 查询租户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IResultOutput> GetPageAsync(PageInput<TenantGetPageDto> input)
    {
        var key = input.Filter?.Name;

        var list = await _tenantRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
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

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Transaction]
    public virtual async Task<IResultOutput> AddAsync(TenantAddInput input)
    {
        if (await _tenantRepository.Select.AnyAsync(a => a.Name == input.Name))
        {
            return ResultOutput.NotOk($"企业名称已存在");
        }

        if (await _tenantRepository.Select.AnyAsync(a => a.Code == input.Code))
        {
            return ResultOutput.NotOk($"企业编码已存在");
        }

        //添加租户
        TenantEntity entity = Mapper.Map<TenantEntity>(input);
        TenantEntity tenant = await _tenantRepository.InsertAsync(entity);
        long tenantId = tenant.Id;

        //添加部门
        var org = new OrgEntity
        {
            TenantId = tenantId,
            Name = input.Name,
            Code = input.Code,
            ParentId = 0,
            MemberCount = 1
        };
        await _orgRepository.InsertAsync(org);

        //添加主管理员
        string pwd = MD5Encrypt.Encrypt32(_appConfig.DefaultPassword);
        var user = new UserEntity
        {
            TenantId = tenantId,
            UserName = input.Phone,
            Password = pwd,
            Name = input.RealName,
            Mobile = input.Phone,
            Email = input.Email,
            Status = UserStatus.Enabled
        };
        await _userRepository.InsertAsync(user);

        long userId = user.Id;

        //添加员工
        var emp = new StaffEntity
        {
            Id = userId,
            TenantId = tenantId
        };
        await _staffRepository.InsertAsync(emp);

        //添加用户部门
        var userOrg = new UserOrgEntity
        {
            UserId = userId,
            OrgId = org.Id
        };
        await _userOrgRepository.InsertAsync(userOrg);

        //添加角色
        var role = new RoleEntity
        {
            TenantId = tenantId,
            Name = "主管理员",
            Code = "admin"
        };
        await _roleRepository.InsertAsync(role);

        //添加用户角色
        var userRole = new UserRoleEntity()
        {
            UserId = userId,
            RoleId = role.Id
        };
        await _userRoleRepository.InsertAsync(userRole);

        //更新租户的用户
        tenant.UserId = userId;
        await _tenantRepository.UpdateAsync(tenant);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Transaction]
    public virtual async Task<IResultOutput> DeleteAsync(long id)
    {
        //删除角色权限
        await _rolePermissionRepository.Where(a => a.Role.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

        //删除用户角色
        await _userRoleRepository.Where(a => a.User.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

        //删除员工
        await _staffRepository.Where(a => a.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

        //删除用户部门
        await _userOrgRepository.Where(a => a.User.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

        //删除用户
        await _userRepository.Where(a => a.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

        //删除角色
        await _roleRepository.Where(a => a.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

        //删除租户
        await _tenantRepository.DeleteAsync(id);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Transaction]
    public virtual async Task<IResultOutput> SoftDeleteAsync(long id)
    {
        //删除用户
        await _userRepository.SoftDeleteAsync(a => a.TenantId == id, "Tenant");

        //删除角色
        await _roleRepository.SoftDeleteAsync(a => a.TenantId == id, "Tenant");

        //删除租户
        var result = await _tenantRepository.SoftDeleteAsync(id);

        return ResultOutput.Result(result);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [Transaction]
    public virtual async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
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