using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Api;
using ZhonTai.Admin.Domain.PermissionApi;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Services.Auth.Dto;
using ZhonTai.Admin.Services.User.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Core.Helpers;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Domain.Employee;
using ZhonTai.Admin.Domain;
using ZhonTai.Admin.Domain.Organization;
using System.Data;

namespace ZhonTai.Admin.Services.User;

/// <summary>
/// 用户服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class UserService : BaseService, IUserService, IDynamicApi
{
    private AppConfig _appConfig => LazyGetRequiredService<AppConfig>();
    private IUserRepository _userRepository => LazyGetRequiredService<IUserRepository>();
    private IRepositoryBase<UserRoleEntity> _userRoleRepository => LazyGetRequiredService<IRepositoryBase<UserRoleEntity>>();
    private ITenantRepository _tenantRepository => LazyGetRequiredService<ITenantRepository>();
    private IApiRepository _apiRepository => LazyGetRequiredService<IApiRepository>();
    private IEmployeeRepository _employeeRepository => LazyGetRequiredService<IEmployeeRepository>();
    private IRepositoryBase<EmployeeOrganizationEntity> _employeeOrganizationRepository => LazyGetRequiredService<IRepositoryBase<EmployeeOrganizationEntity>>();

    public UserService()
    {
    }

    /// <summary>
    /// 查询用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetAsync(long id)
    {
        var output = await _userRepository.Select
        .WhereDynamic(id)
        .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Id = b.Id, Name = b.Name }))
        .IncludeMany(a => a.Emp.Orgs.Select(b => new OrganizationEntity { Id = b.Id, Name = b.Name }))
        .ToOneAsync(a=>new
        {
            a.Id,
            a.Version,
            a.UserName,
            a.Name,
            a.Mobile,
            a.Email,
            a.Roles,
            Emp = new
            {
                a.Emp.Version,
                a.Emp.MainOrgId,
                a.Emp.Orgs
            }
        });

        return ResultOutput.Ok(output);
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IResultOutput> GetPageAsync(PageInput<long?> input)
    {
        var list = await _userRepository.Select
        .WhereIf(input.Filter.HasValue && input.Filter > 0, a=>a.Emp.MainOrgId == input.Filter)
        .WhereDynamicFilter(input.DynamicFilter)
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Name = b.Name }))
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a=>new UserGetPageOutput { Roles = a.Roles });

        var data = new PageOutput<UserGetPageOutput>()
        {
            List = Mapper.Map<List<UserGetPageOutput>>(list),
            Total = total
        };

        return ResultOutput.Ok(data);
    }

    /// <summary>
    /// 查询登录用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResultOutput<AuthLoginOutput>> GetLoginUserAsync(long id)
    {
        var output = new ResultOutput<AuthLoginOutput>();
        var entityDto = await _userRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(id).ToOneAsync<AuthLoginOutput>();
        if (_appConfig.Tenant && entityDto?.TenantId.Value > 0)
        {
            var tenant = await _tenantRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(entityDto.TenantId).ToOneAsync(a => new { a.TenantType, a.DataIsolationType });
            if (null != tenant)
            {
                entityDto.TenantType = tenant.TenantType;
                entityDto.DataIsolationType = tenant.DataIsolationType;
            }
        }
        return output.Ok(entityDto);
    }

    /// <summary>
    /// 查询用户基本信息
    /// </summary>
    /// <returns></returns>
    public async Task<IResultOutput> GetBasicAsync()
    {
        if (!(User?.Id > 0))
        {
            return ResultOutput.NotOk("未登录！");
        }

        var data = await _userRepository.GetAsync<UserUpdateBasicInput>(User.Id);
        return ResultOutput.Ok(data);
    }

    /// <summary>
    /// 查询用户权限信息
    /// </summary>
    /// <returns></returns>
    public async Task<IList<UserPermissionsOutput>> GetPermissionsAsync()
    {
        var key = string.Format(CacheKeys.UserPermissions, User.Id);
        var result = await Cache.GetOrSetAsync(key, async () =>
        {
            return await _apiRepository
            .Where(a => _userRoleRepository.Orm.Select<UserRoleEntity, RolePermissionEntity, PermissionApiEntity>()
            .InnerJoin((b, c, d) => b.RoleId == c.RoleId && b.UserId == User.Id)
            .InnerJoin((b, c, d) => c.PermissionId == d.PermissionId)
            .Where((b, c, d) => d.ApiId == a.Id).Any())
            .ToListAsync<UserPermissionsOutput>();
        });
        return result;
    }

    /// <summary>
    /// 新增用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> AddAsync(UserAddInput input)
    {
        if (await _userRepository.Select.AnyAsync(a => a.UserName == input.UserName))
        {
            return ResultOutput.NotOk($"账号已存在");
        }

        if (input.Mobile.NotNull() && await _userRepository.Select.AnyAsync(a => a.Mobile == input.Mobile))
        {
            return ResultOutput.NotOk($"手机号已存在");
        }

        if (input.Email.NotNull() && await _userRepository.Select.AnyAsync(a => a.Email == input.Email))
        {
            return ResultOutput.NotOk($"邮箱已存在");
        }

        // 用户信息
        if (input.Password.IsNull())
        {
            input.Password = _appConfig.DefaultPassword;
        }

        input.Password = MD5Encrypt.Encrypt32(input.Password);

        var entity = Mapper.Map<UserEntity>(input);
        var user = await _userRepository.InsertAsync(entity);

        if (!(user?.Id > 0))
        {
            return ResultOutput.NotOk("新增用户失败");
        }

        //用户角色
        if (input.RoleIds != null && input.RoleIds.Any())
        {
            var roles = input.RoleIds.Select(a => new UserRoleEntity { UserId = user.Id, RoleId = a });
            await _userRoleRepository.InsertAsync(roles);
        }

        // 员工信息
        var emp = Mapper.Map<EmployeeEntity>(input.Emp);
        emp.Id = user.Id;
        await _employeeRepository.InsertAsync(emp);

        //所属部门
        if (input.Emp.OrgIds != null && input.Emp.OrgIds.Any())
        {
            var organizations = input.Emp.OrgIds.Select(organizationId => new EmployeeOrganizationEntity
            {
                EmployeeId = emp.Id,
                OrganizationId = organizationId
            });
            await _employeeOrganizationRepository.InsertAsync(organizations);
        }

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 修改用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> UpdateAsync(UserUpdateInput input)
    {
        var user = await _userRepository.GetAsync(input.Id);
        if (!(user?.Id > 0))
        {
            return ResultOutput.NotOk("用户不存在");
        }

        if (await _userRepository.Select.AnyAsync(a => a.Id != input.Id && a.UserName == input.UserName))
        {
            return ResultOutput.NotOk($"账号已存在");
        }

        if (input.Mobile.NotNull() && await _userRepository.Select.AnyAsync(a => a.Id != input.Id && a.Mobile == input.Mobile))
        {
            return ResultOutput.NotOk($"手机号已存在");
        }

        if (input.Email.NotNull() && await _userRepository.Select.AnyAsync(a => a.Id != input.Id && a.Email == input.Email))
        {
            return ResultOutput.NotOk($"邮箱已存在");
        }

        Mapper.Map(input, user);
        await _userRepository.UpdateAsync(user);

        // 用户角色
        await _userRoleRepository.DeleteAsync(a => a.UserId == user.Id);
        if (input.RoleIds != null && input.RoleIds.Any())
        {
            var roles = input.RoleIds.Select(a => new UserRoleEntity { UserId = user.Id, RoleId = a });
            await _userRoleRepository.InsertAsync(roles);
        }

        // 员工信息
        var emp = await _employeeRepository.GetAsync(user.Id);
        if(emp == null)
        {
            emp = new EmployeeEntity();
        }
        Mapper.Map(input.Emp, emp);
        emp.Id = user.Id;

        await _employeeRepository.InsertOrUpdateAsync(emp);

        //所属部门
        await _employeeOrganizationRepository.DeleteAsync(a => a.EmployeeId == emp.Id);
        if (input.Emp.OrgIds != null && input.Emp.OrgIds.Any())
        {
            var organizations = input.Emp.OrgIds.Select(organizationId => new EmployeeOrganizationEntity
            {
                EmployeeId = emp.Id,
                OrganizationId = organizationId
            });
            await _employeeOrganizationRepository.InsertAsync(organizations);
        }

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 更新用户基本信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> UpdateBasicAsync(UserUpdateBasicInput input)
    {
        var entity = await _userRepository.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        var result = (await _userRepository.UpdateAsync(entity)) > 0;

        //清除用户缓存
        await Cache.DelAsync(string.Format(CacheKeys.UserInfo, input.Id));

        return ResultOutput.Result(result);
    }

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> ChangePasswordAsync(UserChangePasswordInput input)
    {
        if (input.ConfirmPassword != input.NewPassword)
        {
            return ResultOutput.NotOk("新密码和确认密码不一致！");
        }

        var entity = await _userRepository.GetAsync(input.Id);
        var oldPassword = MD5Encrypt.Encrypt32(input.OldPassword);
        if (oldPassword != entity.Password)
        {
            return ResultOutput.NotOk("旧密码不正确！");
        }

        input.Password = MD5Encrypt.Encrypt32(input.NewPassword);

        entity = Mapper.Map(input, entity);
        var result = (await _userRepository.UpdateAsync(entity)) > 0;

        return ResultOutput.Result(result);
    }

    /// <summary>
    /// 彻底删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> DeleteAsync(long id)
    {
        //删除员工所属部门
        await _employeeOrganizationRepository.DeleteAsync(a => a.EmployeeId == id);
        //删除员工
        await _employeeRepository.DeleteAsync(a => a.Id == id);
        //删除用户角色
        await _userRoleRepository.DeleteAsync(a => a.UserId == id);
        //删除用户
        await _userRepository.DeleteAsync(a => a.Id == id);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 批量彻底删除用户
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> BatchDeleteAsync(long[] ids)
    {
        //删除员工所属部门
        await _employeeOrganizationRepository.DeleteAsync(a => ids.Contains(a.EmployeeId));
        //删除员工
        await _employeeRepository.DeleteAsync(a => ids.Contains(a.Id));
        //删除用户角色
        await _userRoleRepository.DeleteAsync(a => ids.Contains(a.UserId));
        //删除用户
        await _userRepository.DeleteAsync(a => ids.Contains(a.Id));

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> SoftDeleteAsync(long id)
    {
        await _employeeOrganizationRepository.DeleteAsync(a => a.EmployeeId == id);
        await _employeeRepository.SoftDeleteAsync(a => a.Id == id);
        await _userRoleRepository.DeleteAsync(a => a.UserId == id);
        await _userRepository.SoftDeleteAsync(id);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 批量删除用户
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
    {
        await _employeeOrganizationRepository.DeleteAsync(a => ids.Contains(a.EmployeeId));
        await _employeeRepository.SoftDeleteAsync(a => ids.Contains(a.Id));
        await _userRoleRepository.DeleteAsync(a => ids.Contains(a.UserId));
        await _userRepository.SoftDeleteAsync(ids);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    [Login]
    public async Task<IResultOutput> AvatarUpload([FromForm] IFormFile file)
    {
        var uploadConfig = LazyGetRequiredService<IOptionsMonitor<UploadConfig>>().CurrentValue;
        var uploadHelper = LazyGetRequiredService<UploadHelper>();
        var config = uploadConfig.Avatar;
        var res = await uploadHelper.UploadAsync(file, config, new { User.Id });
        if (res.Success)
        {
            return ResultOutput.Ok(res.Data.FileRelativePath);
        }

        return ResultOutput.NotOk(res.Msg ?? "上传失败！");
    }
}