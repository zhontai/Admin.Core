using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;
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
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Domain.UserStaff;
using ZhonTai.Admin.Domain.Org;
using System.Data;
using ZhonTai.Admin.Domain.TenantPermission;
using FreeSql;
using ZhonTai.Admin.Domain.User.Dto;
using ZhonTai.Admin.Domain.RoleOrg;
using ZhonTai.Admin.Domain.UserOrg;
using Microsoft.AspNetCore.Identity;
using ZhonTai.Admin.Services.File;
using System.Linq.Expressions;
using System;
using ZhonTai.Admin.Domain.PkgPermission;
using ZhonTai.Admin.Domain.TenantPkg;
using ZhonTai.Admin.Core;
using Newtonsoft.Json;
using ZhonTai.Admin.Services.Auth;
using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.User;

/// <summary>
/// 用户服务
/// </summary>
[Order(10)]
[DynamicApi(Area = AdminConsts.AreaName)]
public partial class UserService : BaseService, IUserService, IDynamicApi
{
    private AppConfig _appConfig => LazyGetRequiredService<AppConfig>();
    private IUserRepository _userRepository => LazyGetRequiredService<IUserRepository>();
    private IOrgRepository _orgRepository => LazyGetRequiredService<IOrgRepository>();
    private ITenantRepository _tenantRepository => LazyGetRequiredService<ITenantRepository>();
    private IApiRepository _apiRepository => LazyGetRequiredService<IApiRepository>();
    private IUserStaffRepository _staffRepository => LazyGetRequiredService<IUserStaffRepository>();
    private IUserRoleRepository _userRoleRepository => LazyGetRequiredService<IUserRoleRepository>();
    private IRoleOrgRepository _roleOrgRepository => LazyGetRequiredService<IRoleOrgRepository>();
    private IUserOrgRepository _userOrgRepository => LazyGetRequiredService<IUserOrgRepository>();
    private IPasswordHasher<UserEntity> _passwordHasher => LazyGetRequiredService<IPasswordHasher<UserEntity>>();
    private IFileService _fileService => LazyGetRequiredService<IFileService>();

    public UserService()
    {
    }

    /// <summary>
    /// 查询用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserGetOutput> GetAsync(long id)
    {
        var userEntity = await _userRepository.Select
        .WhereDynamic(id)
        .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Id = b.Id, Name = b.Name }))
        .IncludeMany(a => a.Orgs.Select(b => new OrgEntity { Id = b.Id, Name = b.Name }))
        .ToOneAsync(a => new
        {
            a.Id,
            a.UserName,
            a.Name,
            a.Mobile,
            a.Email,
            a.Roles,
            a.Orgs,
            a.OrgId,
            a.ManagerUserId,
            ManagerUserName = a.ManagerUser.Name,
            Staff = new
            {
                a.Staff.JobNumber,
                a.Staff.Sex,
                a.Staff.Position,
                a.Staff.Introduce
            }
        });

        var output = Mapper.Map<UserGetOutput>(userEntity);

        return output;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<UserGetPageOutput>> GetPageAsync(PageInput<UserGetPageDto> input)
    {
        var dataPermission = await AppInfo.GetRequiredService<IUserService>().GetDataPermissionAsync();

        var orgId = input.Filter?.OrgId;
        var list = await _userRepository.Select
        .WhereIf(dataPermission.OrgIds.Count > 0, a => _userOrgRepository.Where(b => b.UserId == a.Id && dataPermission.OrgIds.Contains(b.OrgId)).Any())
        .WhereIf(dataPermission.DataScope == DataScope.Self, a => a.CreatedUserId == User.Id)
        .WhereIf(orgId.HasValue && orgId > 0, a => _userOrgRepository.Where(b => b.UserId == a.Id && b.OrgId == orgId).Any())
        .Where(a=>a.Type != UserType.Member)
        .WhereDynamicFilter(input.DynamicFilter)
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Name = b.Name }))
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a => new UserGetPageOutput { Roles = a.Roles });

        if(orgId.HasValue && orgId > 0)
        {
            var managerUserIds = await _userOrgRepository.Select
                .Where(a => a.OrgId == orgId && a.IsManager == true).ToListAsync(a => a.UserId);

            if (managerUserIds.Any())
            {
                var managerUsers = list.Where(a => managerUserIds.Contains(a.Id));
                foreach (var managerUser in managerUsers)
                {
                    managerUser.IsManager = true;
                }
            }
        }

        var data = new PageOutput<UserGetPageOutput>()
        {
            List = Mapper.Map<List<UserGetPageOutput>>(list),
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 查询登录用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<AuthLoginOutput> GetLoginUserAsync(long id)
    {
        var output = await _userRepository.Select.DisableGlobalFilter(FilterNames.Tenant)
            .WhereDynamic(id).ToOneAsync<AuthLoginOutput>();

        if (_appConfig.Tenant && output?.TenantId.Value > 0)
        {
            var tenant = await _tenantRepository.Select.DisableGlobalFilter(FilterNames.Tenant)
                .WhereDynamic(output.TenantId).ToOneAsync<AuthLoginTenantDto>();
            output.Tenant = tenant;
        }
        return output;
    }

    /// <summary>
    /// 获得数据权限
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task<DataPermissionDto> GetDataPermissionAsync()
    {
        if (!(User?.Id > 0))
        {
            return null;
        }

        var key = CacheKeys.DataPermission + User.Id;
        return await Cache.GetOrSetAsync(key, async () =>
        {
            using (_userRepository.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
            {
                var user = await _userRepository.Select
                .IncludeMany(a => a.Roles.Select(b => new RoleEntity
                {
                    Id = b.Id,
                    DataScope = b.DataScope
                }))
                .WhereDynamic(User.Id)
                .ToOneAsync(a => new
                {
                    a.OrgId,
                    a.Roles
                });

                if (user == null)
                    return null;

                //数据范围
                DataScope dataScope = DataScope.Self;
                var customRoleIds = new List<long>();
                user.Roles?.ToList().ForEach(role =>
                {
                    if (role.DataScope == DataScope.Custom)
                    {
                        customRoleIds.Add(role.Id);
                    }
                    else if (role.DataScope <= dataScope)
                    {
                        dataScope = role.DataScope;
                    }
                });

                //部门列表
                var orgIds = new List<long>();
                if (dataScope != DataScope.All)
                {
                    //本部门
                    if (dataScope == DataScope.Dept)
                    {
                        orgIds.Add(user.OrgId);
                    }
                    //本部门和下级部门
                    else if (dataScope == DataScope.DeptWithChild)
                    {
                        orgIds = await _orgRepository
                        .Where(a => a.Id == user.OrgId)
                        .AsTreeCte()
                        .ToListAsync(a => a.Id);
                    }

                    //指定部门
                    if (customRoleIds.Count > 0)
                    {
                        if(dataScope == DataScope.Self)
                        {
                            dataScope = DataScope.Custom;
                        }
                        var customRoleOrgIds = await _roleOrgRepository.Select.Where(a => customRoleIds.Contains(a.RoleId)).ToListAsync(a => a.OrgId);
                        orgIds = orgIds.Concat(customRoleOrgIds).ToList();
                    }
                }

                return new DataPermissionDto
                {
                    OrgId = user.OrgId,
                    OrgIds = orgIds.Distinct().ToList(),
                    DataScope = (User.PlatformAdmin || User.TenantAdmin) ? DataScope.All : dataScope
                };
            }
        });
    }

    /// <summary>
    /// 查询用户基本信息
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<UserGetBasicOutput> GetBasicAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception("未登录！");
        }

        var data = await _userRepository.GetAsync<UserGetBasicOutput>(User.Id);
        data.Mobile = DataMaskHelper.PhoneMask(data.Mobile);
        data.Email = DataMaskHelper.EmailMask(data.Email);
        return data;
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
                var db = cloud.Use(DbKeys.AppDb);

                var tenantPermissions = await db.Select<ApiEntity>()
                .Where(a => db.Select<TenantPermissionEntity, PermissionApiEntity>()
                .InnerJoin((b, c) => b.PermissionId == c.PermissionId && b.TenantId == User.TenantId)
                .Where((b, c) => c.ApiId == a.Id).Any())
                .ToListAsync<UserPermissionsOutput>();

                var pkgPermissions = await db.Select<ApiEntity>()
                .Where(a => db.Select<TenantPkgEntity, PkgPermissionEntity, PermissionApiEntity>()
                .InnerJoin((b, c, d) => b.PkgId == c.PkgId && c.PermissionId == d.PermissionId && b.TenantId == User.TenantId)
                .Where((b, c, d) => d.ApiId == a.Id).Any())
                .ToListAsync<UserPermissionsOutput>();

                return tenantPermissions.Union(pkgPermissions).Distinct().ToList();
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
    [AdminTransaction]
    public virtual async Task<long> AddAsync(UserAddInput input)
    {
        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRepository.Select.Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception($"账号已存在");
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception($"手机号已存在");
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception($"邮箱已存在");
            }
        }

        // 用户信息
        if (input.Password.IsNull())
        {
            input.Password = _appConfig.DefaultPassword;
        }

        var entity = Mapper.Map<UserEntity>(input);
        entity.Type = UserType.DefaultUser;
        if (_appConfig.PasswordHasher)
        {
            entity.Password = _passwordHasher.HashPassword(entity, input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        var user = await _userRepository.InsertAsync(entity);
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
        var staff = input.Staff == null ? new UserStaffEntity() : Mapper.Map<UserStaffEntity>(input.Staff);
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

        return userId;
    }

    /// <summary>
    /// 修改用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateAsync(UserUpdateInput input)
    {
        if (input.Id == input.ManagerUserId)
        {
            throw ResultOutput.Exception("直属主管不能是自己");
        }

        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRepository.Select.Where(a => a.Id != input.Id).Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception($"账号已存在");
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception($"手机号已存在");
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception($"邮箱已存在");
            }
        }

        var user = await _userRepository.GetAsync(input.Id);
        if (!(user?.Id > 0))
        {
            throw ResultOutput.Exception("用户不存在");
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
        staff ??= new UserStaffEntity();
        Mapper.Map(input.Staff, staff);
        staff.Id = userId;
        await _staffRepository.InsertOrUpdateAsync(staff);

        //所属部门
        var orgIds = await _userOrgRepository.Select.Where(a => a.UserId == userId).ToListAsync(a => a.OrgId);
        var insertOrgIds = input.OrgIds.Except(orgIds);

        var deleteOrgIds = orgIds.Except(input.OrgIds);
        if (deleteOrgIds != null && deleteOrgIds.Any())
        {
            await _userOrgRepository.DeleteAsync(a => a.UserId == userId && deleteOrgIds.Contains(a.OrgId));
        }
            
        if (insertOrgIds != null && insertOrgIds.Any())
        {
            var orgs = insertOrgIds.Select(orgId => new UserOrgEntity
            {
                UserId = userId,
                OrgId = orgId
            }).ToList();
            await _userOrgRepository.InsertAsync(orgs);
        }

        await Cache.DelAsync(CacheKeys.DataPermission + user.Id);
    }

    /// <summary>
    /// 新增会员
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<long> AddMemberAsync(UserAddMemberInput input)
    {
        using (_userRepository.DataFilter.DisableAll())
        {
            Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
            where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
                .Or(input.Email.NotNull(), a => a.Email == input.Email);

            var existsUser = await _userRepository.Select.Where(where)
                .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

            if (existsUser != null)
            {
                if (existsUser.UserName == input.UserName)
                {
                    throw ResultOutput.Exception($"账号已存在");
                }

                if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
                {
                    throw ResultOutput.Exception($"手机号已存在");
                }

                if (input.Email.NotNull() && existsUser.Email == input.Email)
                {
                    throw ResultOutput.Exception($"邮箱已存在");
                }
            }

            // 用户信息
            if (input.Password.IsNull())
            {
                input.Password = _appConfig.DefaultPassword;
            }

            var entity = Mapper.Map<UserEntity>(input);
            entity.Type = UserType.Member;
            if (_appConfig.PasswordHasher)
            {
                entity.Password = _passwordHasher.HashPassword(entity, input.Password);
                entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
            }
            else
            {
                entity.Password = MD5Encrypt.Encrypt32(input.Password);
                entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
            }
            var user = await _userRepository.InsertAsync(entity);

            return user.Id;
        }
    }

    /// <summary>
    /// 修改会员
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateMemberAsync(UserUpdateMemberInput input)
    {
        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRepository.Select.Where(a => a.Id != input.Id).Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception($"账号已存在");
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception($"手机号已存在");
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception($"邮箱已存在");
            }
        }

        var user = await _userRepository.GetAsync(input.Id);
        if (!(user?.Id > 0))
        {
            throw ResultOutput.Exception("用户不存在");
        }

        Mapper.Map(input, user);
        await _userRepository.UpdateAsync(user);
    }

    /// <summary>
    /// 更新用户基本信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Login]
    public async Task UpdateBasicAsync(UserUpdateBasicInput input)
    {
        var entity = await _userRepository.GetAsync(User.Id);
        entity = Mapper.Map(input, entity);
        await _userRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Login]
    public async Task ChangePasswordAsync(UserChangePasswordInput input)
    {
        if (input.ConfirmPassword != input.NewPassword)
        {
            throw ResultOutput.Exception("新密码和确认密码不一致");
        }

        var entity = await _userRepository.GetAsync(User.Id);
        var oldPassword = MD5Encrypt.Encrypt32(input.OldPassword);
        if (oldPassword != entity.Password)
        {
            throw ResultOutput.Exception("旧密码不正确");
        }

        entity.Password = MD5Encrypt.Encrypt32(input.NewPassword);
        await _userRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<string> ResetPasswordAsync(UserResetPasswordInput input)
    {
        var entity = await _userRepository.GetAsync(input.Id);
        var password = input.Password;
        if (password.IsNull())
        {
            password = _appConfig.DefaultPassword;
        }
        if (password.IsNull())
        {
            password = "111111";
        }
        if (_appConfig.PasswordHasher)
        {
            entity.Password = _passwordHasher.HashPassword(entity, password);
            entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(password);
            entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        await _userRepository.UpdateAsync(entity);
        return password;
    }

    /// <summary>
    /// 设置主管
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetManagerAsync(UserSetManagerInput input)
    {
        var entity = await _userOrgRepository.Where(a => a.UserId == input.UserId && a.OrgId == input.OrgId).FirstAsync();
        entity.IsManager = input.IsManager;
        await _userOrgRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 设置启用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetEnableAsync(UserSetEnableInput input)
    {
        var entity = await _userRepository.GetAsync(input.UserId);
        if (entity.Type == UserType.PlatformAdmin)
        {
            throw ResultOutput.Exception("平台管理员禁止禁用");
        }
        if (entity.Type == UserType.TenantAdmin)
        {
            throw ResultOutput.Exception("企业管理员禁止禁用");
        }
        entity.Enabled = input.Enabled;
        await _userRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var user = await _userRepository.Select.WhereDynamic(id).ToOneAsync(a => new { a.Type });
        if(user == null)
        {
            throw ResultOutput.Exception("用户不存在");
        }

        if(user.Type == UserType.PlatformAdmin)
        {
            throw ResultOutput.Exception($"平台管理员禁止删除");
        }

        if (user.Type == UserType.TenantAdmin)
        {
            throw ResultOutput.Exception($"企业管理员禁止删除");
        }

        //删除用户角色
        await _userRoleRepository.DeleteAsync(a => a.UserId == id);
        //删除用户所属部门
        await _userOrgRepository.DeleteAsync(a => a.UserId == id);
        //删除员工
        await _staffRepository.DeleteAsync(a => a.Id == id);
        //删除用户
        await _userRepository.DeleteAsync(a => a.Id == id);

        //删除用户数据权限缓存
        await Cache.DelAsync(CacheKeys.DataPermission + id);
    }

    /// <summary>
    /// 批量彻底删除用户
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var admin = await _userRepository.Select.Where(a => ids.Contains(a.Id) && 
        (a.Type == UserType.PlatformAdmin || a.Type == UserType.TenantAdmin)).AnyAsync();

        if (admin)
        {
            throw ResultOutput.Exception("平台管理员禁止删除");
        }
       
        //删除用户角色
        await _userRoleRepository.DeleteAsync(a => ids.Contains(a.UserId));
        //删除用户所属部门
        await _userOrgRepository.DeleteAsync(a => ids.Contains(a.UserId));
        //删除员工
        await _staffRepository.DeleteAsync(a => ids.Contains(a.Id));
        //删除用户
        await _userRepository.DeleteAsync(a => ids.Contains(a.Id));

        foreach (var userId in ids)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        var user = await _userRepository.Select.WhereDynamic(id).ToOneAsync(a => new { a.Type });
        if (user == null)
        {
            throw ResultOutput.Exception("用户不存在");
        }

        if (user.Type == UserType.PlatformAdmin || user.Type == UserType.TenantAdmin)
        {
            throw ResultOutput.Exception("平台管理员禁止删除");
        }

        await _userRoleRepository.DeleteAsync(a => a.UserId == id);
        await _userOrgRepository.DeleteAsync(a => a.UserId == id);
        await _staffRepository.SoftDeleteAsync(a => a.Id == id);
        await _userRepository.SoftDeleteAsync(id);

        await Cache.DelAsync(CacheKeys.DataPermission + id);
    }

    /// <summary>
    /// 批量删除用户
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var admin = await _userRepository.Select.Where(a => ids.Contains(a.Id) && 
        (a.Type == UserType.PlatformAdmin || a.Type == UserType.TenantAdmin)).AnyAsync();

        if (admin)
        {
            throw ResultOutput.Exception("平台管理员禁止删除");
        }

        await _userRoleRepository.DeleteAsync(a => ids.Contains(a.UserId));
        await _userOrgRepository.DeleteAsync(a => ids.Contains(a.UserId));
        await _staffRepository.SoftDeleteAsync(a => ids.Contains(a.Id));
        await _userRepository.SoftDeleteAsync(ids);

        foreach (var userId in ids)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="file"></param>
    /// <param name="autoUpdate"></param>
    /// <returns></returns>
    [HttpPost]
    [Login]
    public async Task<string> AvatarUpload([FromForm] IFormFile file, bool autoUpdate = false)
    {
        var fileInfo = await _fileService.UploadFileAsync(file);
        if (autoUpdate)
        {
            var entity = await _userRepository.GetAsync(User.Id);
            entity.Avatar = fileInfo.LinkUrl;
            await _userRepository.UpdateAsync(entity);
        }
        return fileInfo.LinkUrl;
    }

    /// <summary>
    /// 一键登录用户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> OneClickLoginAsync([Required]string userName)
    {
        if (userName.IsNull())
        {
            throw ResultOutput.Exception("请选择用户");
        }

        using var _ = _userRepository.DataFilter.DisableAll();

        var user = await _userRepository.Select.Where(a => a.UserName == userName).ToOneAsync();

        if(user == null)
        {
            throw ResultOutput.Exception("用户不存在");
        }

        var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);
        if (_appConfig.Tenant)
        {
            var tenant = await _tenantRepository.Select.WhereDynamic(user.TenantId).ToOneAsync<AuthLoginTenantDto>();
            authLoginOutput.Tenant = tenant;
        }

        string token = AppInfo.GetRequiredService<IAuthService>().GetToken(authLoginOutput);

        return new { token }; 
    }
}