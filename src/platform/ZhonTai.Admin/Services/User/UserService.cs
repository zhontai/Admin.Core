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
using ZhonTai.Admin.Domain.Staff;
using ZhonTai.Admin.Domain;
using ZhonTai.Admin.Domain.Org;
using System.Data;
using ZhonTai.Admin.Domain.TenantPermission;
using FreeSql;
using ZhonTai.Admin.Domain.User.Dto;

namespace ZhonTai.Admin.Services.User;

/// <summary>
/// 用户服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class UserService : BaseService, IUserService, IDynamicApi
{
    private AppConfig _appConfig => LazyGetRequiredService<AppConfig>();
    private IUserRepository _userRepository => LazyGetRequiredService<IUserRepository>();
    private IOrgRepository _orgRepository => LazyGetRequiredService<IOrgRepository>();
    private ITenantRepository _tenantRepository => LazyGetRequiredService<ITenantRepository>();
    private IApiRepository _apiRepository => LazyGetRequiredService<IApiRepository>();
    private IStaffRepository _staffRepository => LazyGetRequiredService<IStaffRepository>();
    private IRepositoryBase<UserRoleEntity> _userRoleRepository => LazyGetRequiredService<IRepositoryBase<UserRoleEntity>>();
    private IRepositoryBase<RoleOrgEntity> _roleOrgRepository => LazyGetRequiredService<IRepositoryBase<RoleOrgEntity>>();
    private IRepositoryBase<UserOrgEntity> _userOrgRepository => LazyGetRequiredService<IRepositoryBase<UserOrgEntity>>();

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
        var userEntity = await _userRepository.Select
        .WhereDynamic(id)
        .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Id = b.Id, Name = b.Name }))
        .IncludeMany(a => a.Orgs.Select(b => new OrgEntity { Id = b.Id, Name = b.Name }))
        .ToOneAsync(a => new
        {
            a.Id,
            a.Version,
            a.UserName,
            a.Name,
            a.Mobile,
            a.Email,
            a.Roles,
            a.Orgs,
            a.OrgId,
            Staff = new
            {
                a.Staff.JobNumber,
                a.Staff.Sex,
                a.Staff.Position,
                a.Staff.Introduce,
                a.Staff.Version
            }
        });

        var output = Mapper.Map<UserGetOutput>(userEntity);

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
        var orgId = input.Filter;
        var list = await _userRepository.Select
        .WhereIf(orgId.HasValue && orgId > 0, a => _userOrgRepository.Where(b => b.UserId == a.Id && b.OrgId == orgId).Any())
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
        var entityDto = await _userRepository.Select.DisableGlobalFilter(FilterNames.Tenant)
            .WhereDynamic(id).ToOneAsync<AuthLoginOutput>();

        if (_appConfig.Tenant && entityDto?.TenantId.Value > 0)
        {
            var tenant = await _tenantRepository.Select.DisableGlobalFilter(FilterNames.Tenant)
                .WhereDynamic(entityDto.TenantId).ToOneAsync(a => new { a.TenantType, a.DbKey });
            entityDto.TenantType = tenant.TenantType;
            entityDto.DbKey = tenant.DbKey;
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
        var key = CacheKeys.UserPermissions + User.Id;
        var result = await Cache.GetOrSetAsync(key, async () =>
        {
            if (User.TenantAdmin)
            {
                var cloud = LazyGetRequiredService<FreeSqlCloud>();
                var db = cloud.Use(DbKeys.MasterDb);

                return await db.Select<ApiEntity>()
                .Where(a => db.Select<TenantPermissionEntity, PermissionApiEntity>()
                .InnerJoin((b, c) => b.PermissionId == c.PermissionId && b.TenantId == User.TenantId)
                .Where((b, c) => c.ApiId == a.Id).Any())
                .ToListAsync<UserPermissionsOutput>();
            }


            return await _apiRepository
            .Where(a => _apiRepository.Orm.Select<UserRoleEntity, RolePermissionEntity, PermissionApiEntity>()
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
    public virtual async Task<IResultOutput> AddAsync(UserAddInput input)
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

        var userId = user.Id;

        //用户角色
        if (input.RoleIds != null && input.RoleIds.Any())
        {
            var roles = input.RoleIds.Select(roleId => new UserRoleEntity 
            { 
                UserId = userId, 
                RoleId = roleId 
            }).ToList();
            await _userRoleRepository.InsertAsync(roles);
        }

        // 员工信息
        var staff = Mapper.Map<StaffEntity>(input.Staff);
        staff.Id = userId;
        await _staffRepository.InsertAsync(staff);

        //所属部门
        if (input.OrgIds != null && input.OrgIds.Any())
        {
            var orgs = input.OrgIds.Select(orgId => new UserOrgEntity
            {
                UserId = userId,
                OrgId = orgId
            }).ToList();
            await _userOrgRepository.InsertAsync(orgs);
        }

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 修改用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Transaction]
    public virtual async Task<IResultOutput> UpdateAsync(UserUpdateInput input)
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

        var userId = user.Id;

        // 用户角色
        await _userRoleRepository.DeleteAsync(a => a.UserId == userId);
        if (input.RoleIds != null && input.RoleIds.Any())
        {
            var roles = input.RoleIds.Select(roleId => new UserRoleEntity 
            { 
                UserId = userId, 
                RoleId = roleId 
            }).ToList();
            await _userRoleRepository.InsertAsync(roles);
        }

        // 员工信息
        var staff = await _staffRepository.GetAsync(userId);
        if(staff == null)
        {
            staff = new StaffEntity();
        }
        Mapper.Map(input.Staff, staff);
        staff.Id = userId;
        await _staffRepository.InsertOrUpdateAsync(staff);

        //所属部门
        await _userOrgRepository.DeleteAsync(a => a.UserId == userId);
        if (input.OrgIds != null && input.OrgIds.Any())
        {
            var orgs = input.OrgIds.Select(orgId => new UserOrgEntity
            {
                UserId = userId,
                OrgId = orgId
            }).ToList();
            await _userOrgRepository.InsertAsync(orgs);
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
        await _userRepository.UpdateAsync(entity);

        return ResultOutput.Ok();
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
            return ResultOutput.NotOk("新密码和确认密码不一致");
        }

        var entity = await _userRepository.GetAsync(input.Id);
        var oldPassword = MD5Encrypt.Encrypt32(input.OldPassword);
        if (oldPassword != entity.Password)
        {
            return ResultOutput.NotOk("旧密码不正确");
        }

        input.Password = MD5Encrypt.Encrypt32(input.NewPassword);

        entity = Mapper.Map(input, entity);
        await _userRepository.UpdateAsync(entity);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 彻底删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Transaction]
    public virtual async Task<IResultOutput> DeleteAsync(long id)
    {
        var user = await _userRepository.Select.WhereDynamic(id).ToOneAsync(a => new { a.Type });
        if(user == null)
        {
            return ResultOutput.NotOk("用户不存在");
        }

        if(user.Type == UserType.PlatformAdmin || user.Type == UserType.TenantAdmin)
        {
            return ResultOutput.NotOk("平台管理员禁止删除");
        }

        //删除用户角色
        await _userRoleRepository.DeleteAsync(a => a.UserId == id);
        //删除用户所属部门
        await _userOrgRepository.DeleteAsync(a => a.UserId == id);
        //删除员工
        await _staffRepository.DeleteAsync(a => a.Id == id);
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
    public virtual async Task<IResultOutput> BatchDeleteAsync(long[] ids)
    {
        var admin = await _userRepository.Select.Where(a => ids.Contains(a.Id) && 
        (a.Type == UserType.PlatformAdmin || a.Type == UserType.TenantAdmin)).AnyAsync();

        if (admin)
        {
            return ResultOutput.NotOk("平台管理员禁止删除");
        }
       
        //删除用户角色
        await _userRoleRepository.DeleteAsync(a => ids.Contains(a.UserId));
        //删除用户所属部门
        await _userOrgRepository.DeleteAsync(a => ids.Contains(a.UserId));
        //删除员工
        await _staffRepository.DeleteAsync(a => ids.Contains(a.Id));
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
    public virtual async Task<IResultOutput> SoftDeleteAsync(long id)
    {
        var user = await _userRepository.Select.WhereDynamic(id).ToOneAsync(a => new { a.Type });
        if (user == null)
        {
            return ResultOutput.NotOk("用户不存在");
        }

        if (user.Type == UserType.PlatformAdmin || user.Type == UserType.TenantAdmin)
        {
            return ResultOutput.NotOk("平台管理员禁止删除");
        }

        await _userRoleRepository.DeleteAsync(a => a.UserId == id);
        await _userOrgRepository.DeleteAsync(a => a.UserId == id);
        await _staffRepository.SoftDeleteAsync(a => a.Id == id);
        await _userRepository.SoftDeleteAsync(id);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 批量删除用户
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [Transaction]
    public virtual async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
    {
        var admin = await _userRepository.Select.Where(a => ids.Contains(a.Id) && 
        (a.Type == UserType.PlatformAdmin || a.Type == UserType.TenantAdmin)).AnyAsync();

        if (admin)
        {
            return ResultOutput.NotOk("平台管理员禁止删除");
        }

        await _userRoleRepository.DeleteAsync(a => ids.Contains(a.UserId));
        await _userOrgRepository.DeleteAsync(a => ids.Contains(a.UserId));
        await _staffRepository.SoftDeleteAsync(a => ids.Contains(a.Id));
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