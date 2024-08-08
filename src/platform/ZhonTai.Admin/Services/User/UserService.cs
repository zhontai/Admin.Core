using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Helpers;
using ZhonTai.Admin.Domain.Api;
using ZhonTai.Admin.Domain.PermissionApi;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Domain.UserStaff;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Services.Auth;
using ZhonTai.Admin.Services.Auth.Dto;
using ZhonTai.Admin.Services.User.Dto;
using ZhonTai.Common.Helpers;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Domain.TenantPermission;
using ZhonTai.Admin.Domain.User.Dto;
using ZhonTai.Admin.Domain.RoleOrg;
using ZhonTai.Admin.Domain.UserOrg;
using ZhonTai.Admin.Domain.PkgPermission;
using ZhonTai.Admin.Domain.TenantPkg;
using FreeSql;
using ZhonTai.Admin.Resources;
using ZhonTai.Admin.Domain.Permission;

namespace ZhonTai.Admin.Services.User;

/// <summary>
/// 用户服务
/// </summary>
[Order(10)]
[DynamicApi(Area = AdminConsts.AreaName)]
public partial class UserService : BaseService, IUserService, IDynamicApi
{
    private readonly IUserRepository _userRep;
    private readonly IUserOrgRepository _userOrgRep;
    private readonly IUserRoleRepository _userRoleRep;
    private readonly IUserStaffRepository _userStaffRep;
    private readonly AppConfig _appConfig;
    private readonly UserHelper _userHelper;
    private readonly Lazy<IPasswordHasher<UserEntity>> _passwordHasher;
    private readonly Lazy<IRoleRepository> _roleRep;
    private readonly Lazy<IRolePermissionRepository> _rolePermissionRep;
    private readonly Lazy<IFileService> _fileService;
    private readonly Lazy<IRoleOrgRepository> _roleOrgRep;
    private readonly Lazy<IApiRepository> _apiRep;
    private readonly Lazy<ITenantRepository> _tenantRep;
    private readonly Lazy<IOrgRepository> _orgRep;
    private readonly Lazy<IPermissionRepository> _permissionRep;
    private readonly AdminLocalizer _adminLocalizer;

    public UserService(
        IUserRepository userRep,
        IUserOrgRepository userOrgRep,
        IUserRoleRepository userRoleRep,
        IUserStaffRepository userStaffRep,
        AppConfig appConfig,
        UserHelper userHelper,
        Lazy<IPasswordHasher<UserEntity>> passwordHasher,
        Lazy<IRoleRepository> roleRep,
        Lazy<IRolePermissionRepository> rolePermissionRep,
        Lazy<IFileService> fileService,
        Lazy<IRoleOrgRepository> roleOrgRep,
        Lazy<IApiRepository> apiRep,
        Lazy<ITenantRepository> tenantRep,
        Lazy<IOrgRepository> orgRep,
        Lazy<IPermissionRepository> permissionRep,
        AdminLocalizer adminLocalizer
    )
    {
        _appConfig = appConfig;
        _userHelper = userHelper;
        _passwordHasher = passwordHasher;
        _roleRep = roleRep;
        _rolePermissionRep = rolePermissionRep;
        _fileService = fileService;
        _userRep = userRep;
        _userOrgRep = userOrgRep;
        _roleOrgRep = roleOrgRep;
        _userRoleRep = userRoleRep;
        _userStaffRep = userStaffRep;
        _apiRep = apiRep;
        _tenantRep = tenantRep;
        _orgRep = orgRep;
        _permissionRep = permissionRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserGetOutput> GetAsync(long id)
    {
        var userEntity = await _userRep.Select
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
        var dataPermission = User.DataPermission;

        var orgId = input.Filter?.OrgId;
        var list = await _userRep.Select
        .WhereIf(dataPermission != null && dataPermission.OrgIds.Count > 0, a => _userOrgRep.Where(b => b.UserId == a.Id && dataPermission.OrgIds.Contains(b.OrgId)).Any())
        .WhereIf(dataPermission != null && dataPermission.DataScope == DataScope.Self, a => a.CreatedUserId == User.Id)
        .WhereIf(orgId.HasValue && orgId > 0, a => _userOrgRep.Where(b => b.UserId == a.Id && b.OrgId == orgId).Any())
        .Where(a => a.Type != UserType.Member)
        .WhereDynamicFilter(input.DynamicFilter)
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Name = b.Name }))
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a => new UserGetPageOutput { Roles = a.Roles });

        if (orgId.HasValue && orgId > 0)
        {
            var managerUserIds = await _userOrgRep.Select
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
        var output = await _userRep.Select
            .DisableGlobalFilter(FilterNames.Tenant)
            .WhereDynamic(id)
            .ToOneAsync<AuthLoginOutput>();

        if (_appConfig.Tenant && output?.TenantId.Value > 0)
        {
            var tenant = await _tenantRep.Value.Select
                .DisableGlobalFilter(FilterNames.Tenant)
                .WhereDynamic(output.TenantId)
                .ToOneAsync<AuthLoginTenantDto>();

            output.Tenant = tenant;
        }
        return output;
    }

    /// <summary>
    /// 获得数据权限
    /// </summary>
    /// <param name="apiPath"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataPermissionDto> GetDataPermissionAsync(string? apiPath)
    {
        if (!(User?.Id > 0))
        {
            return null;
        }

        return await Cache.GetOrSetAsync(CacheKeys.GetDataPermissionKey(User.Id, apiPath), async () =>
        {
            using var _ = _userRep.DataFilter.Disable(FilterNames.Self, FilterNames.Data);

            var user = await _userRep.Select
            .WhereDynamic(User.Id)
            .ToOneAsync(a => new { a.OrgId });

            if (user == null)
                return null;

            var orgId = user.OrgId;

            //查询角色
            var roleRepository = _roleRep.Value;
            var rolePermissionRepository = _rolePermissionRep.Value;
            var roles = await roleRepository.Select
            .InnerJoin<UserRoleEntity>((a, b) => a.Id == b.RoleId && b.UserId == User.Id)
            .WhereIf(apiPath.NotNull(), r => rolePermissionRepository.Select
                .From<PermissionApiEntity, ApiEntity>((s, b, c) =>
                    s.InnerJoin(a => a.PermissionId == b.PermissionId)
                    .InnerJoin(a => b.ApiId == c.Id && c.Path == apiPath)
                ).ToList((a, b, c) => a.RoleId).Contains(r.Id))
            .ToListAsync(a => new { a.Id, a.DataScope });

            //数据范围
            DataScope dataScope = DataScope.Self;
            var customRoleIds = new List<long>();
            roles?.ToList().ForEach(role =>
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
                    orgIds.Add(orgId);
                }
                //本部门和下级部门
                else if (dataScope == DataScope.DeptWithChild)
                {
                    orgIds = await _orgRep.Value
                    .Where(a => a.Id == orgId)
                    .AsTreeCte()
                    .ToListAsync(a => a.Id);
                }

                //指定部门
                if (customRoleIds.Count > 0)
                {
                    if (dataScope == DataScope.Self)
                    {
                        dataScope = DataScope.Custom;
                    }

                    var customRoleOrgIds = await _roleOrgRep.Value.Select
                    .Where(a => customRoleIds.Contains(a.RoleId))
                    .ToListAsync(a => a.OrgId);

                    orgIds = orgIds.Concat(customRoleOrgIds).ToList();
                }
            }

            return new DataPermissionDto
            {
                OrgId = orgId,
                OrgIds = orgIds.Distinct().ToList(),
                DataScope = (User.PlatformAdmin || User.TenantAdmin) ? DataScope.All : dataScope
            };
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
            throw ResultOutput.Exception(_adminLocalizer["未登录"]);
        }

        var user = await _userRep.GetAsync<UserGetBasicOutput>(User.Id);

        if (user == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["用户不存在"]);
        }

        user.Mobile = DataMaskHelper.PhoneMask(user.Mobile);
        user.Email = DataMaskHelper.EmailMask(user.Email);

        return user;
    }

    /// <summary>
    /// 查询用户权限信息
    /// </summary>
    /// <returns></returns>
    public async Task<UserGetPermissionOutput> GetPermissionAsync()
    {
        var key = CacheKeys.UserPermission + User.Id;
        var result = await Cache.GetOrSetAsync(key, async () =>
        {
            var output = new UserGetPermissionOutput();
            if (User.TenantAdmin)
            {
                var cloud = LazyGetRequiredService<FreeSqlCloud>();
                var db = cloud.Use(DbKeys.AppDb);

                //租户接口
                var tenantApis = await db.Select<ApiEntity>()
                .Where(a => db.Select<TenantPermissionEntity, PermissionApiEntity>()
                    .InnerJoin((b, c) => b.PermissionId == c.PermissionId && b.TenantId == User.TenantId)
                    .Where((b, c) => c.ApiId == a.Id).Any())
                .ToListAsync<UserGetPermissionOutput.Models.ApiModel>();

                //租户权限点编码
                var tenantCodes = await db.Select<PermissionEntity>()
                .Where(p => db.Select<TenantPermissionEntity>()
                    .InnerJoin(tp => tp.PermissionId == p.Id && tp.TenantId == User.TenantId).Any()
                    && p.Type == PermissionType.Dot && p.Code != null && p.Code != "")
                .ToListAsync(p => p.Code);

                //套餐接口
                var pkgApis = await db.Select<ApiEntity>()
                .Where(a => db.Select<TenantPkgEntity, PkgPermissionEntity, PermissionApiEntity>()
                    .InnerJoin((b, c, d) => b.PkgId == c.PkgId && c.PermissionId == d.PermissionId && b.TenantId == User.TenantId)
                    .Where((b, c, d) => d.ApiId == a.Id).Any())
                .ToListAsync<UserGetPermissionOutput.Models.ApiModel>();

                //套餐权限点编码
                var pkgCodes = await db.Select<PermissionEntity>()
                .Where(p => db.Select<TenantPkgEntity, PkgPermissionEntity>()
                    .InnerJoin((tp, pp) => tp.PkgId == pp.PkgId && pp.PermissionId == p.Id && tp.TenantId == User.TenantId).Any()
                    && p.Type == PermissionType.Dot && p.Code != null && p.Code != "")
                .ToListAsync(p => p.Code);

                output.Apis = tenantApis.Union(pkgApis).Distinct().ToList();

                output.Codes = tenantCodes.Union(pkgCodes).Distinct().ToList();

                return output;
            }

            //角色接口
            output.Apis = await _apiRep.Value
            .Where(a => _apiRep.Value.Orm.Select<UserRoleEntity, RolePermissionEntity, PermissionApiEntity>()
                .InnerJoin((b, c, d) => b.RoleId == c.RoleId && b.UserId == User.Id)
                .InnerJoin((b, c, d) => c.PermissionId == d.PermissionId)
                .Where((b, c, d) => d.ApiId == a.Id).Any())
            .ToListAsync<UserGetPermissionOutput.Models.ApiModel>();

            //角色权限点编码
            output.Codes = await _permissionRep.Value.Where(p => _permissionRep.Value.Orm.Select<UserRoleEntity, RolePermissionEntity>()
                .InnerJoin((ur, rp) => ur.RoleId == rp.RoleId && ur.UserId == User.Id
                && rp.PermissionId == p.Id && p.Type == PermissionType.Dot && p.Code != null && p.Code != "").Any()
            ).ToListAsync(p => p.Code);
            output.Codes = output.Codes.Distinct().ToList();

            return output;
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
        //检查密码
        if (input.Password.IsNull())
        {
            input.Password = _appConfig.DefaultPassword;
        }
        _userHelper.CheckPassword(input.Password);

        using var _ = _userRep.DataFilter.DisableAll();

        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.Select.Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception(_adminLocalizer["账号已存在"]);
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception(_adminLocalizer["手机号已存在"]);
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception(_adminLocalizer["邮箱已存在"]);
            }
        }

        // 用户信息
        var entity = Mapper.Map<UserEntity>(input);
        entity.Type = UserType.DefaultUser;
        if (_appConfig.PasswordHasher)
        {
            entity.Password = _passwordHasher.Value.HashPassword(entity, input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        var user = await _userRep.InsertAsync(entity);
        var userId = user.Id;

        //用户角色
        if (input.RoleIds != null && input.RoleIds.Any())
        {
            var roles = input.RoleIds.Select(roleId => new UserRoleEntity
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();
            await _userRoleRep.InsertAsync(roles);
        }

        // 员工信息
        var staff = input.Staff == null ? new UserStaffEntity() : Mapper.Map<UserStaffEntity>(input.Staff);
        staff.Id = userId;
        await _userStaffRep.InsertAsync(staff);

        //所属部门
        if (input.OrgIds != null && input.OrgIds.Any())
        {
            var orgs = input.OrgIds.Select(orgId => new UserOrgEntity
            {
                UserId = userId,
                OrgId = orgId
            }).ToList();
            await _userOrgRep.InsertAsync(orgs);
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
            throw ResultOutput.Exception(_adminLocalizer["直属主管不能是自己"]);
        }

        using var _ = _userRep.DataFilter.DisableAll();

        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.Select.Where(a => a.Id != input.Id).Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception(_adminLocalizer["账号已存在"]);
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception(_adminLocalizer["手机号已存在"]);
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception(_adminLocalizer["邮箱已存在"]);
            }
        }

        var user = await _userRep.GetAsync(input.Id);
        if (!(user?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["用户不存在"]);
        }

        Mapper.Map(input, user);
        await _userRep.UpdateAsync(user);

        var userId = user.Id;

        // 用户角色
        await _userRoleRep.DeleteAsync(a => a.UserId == userId);
        if (input.RoleIds != null && input.RoleIds.Any())
        {
            var roles = input.RoleIds.Select(roleId => new UserRoleEntity
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();
            await _userRoleRep.InsertAsync(roles);
        }

        // 员工信息
        var staff = await _userStaffRep.GetAsync(userId);
        var existsStaff = staff != null;
        staff ??= new UserStaffEntity();
        Mapper.Map(input.Staff, staff);
        staff.Id = userId;
        if (existsStaff)
        {
            await _userStaffRep.UpdateAsync(staff);
        }
        else
        {
            await _userStaffRep.InsertAsync(staff);
        }

        //所属部门
        var orgIds = await _userOrgRep.Select.Where(a => a.UserId == userId).ToListAsync(a => a.OrgId);
        var insertOrgIds = input.OrgIds.Except(orgIds);

        var deleteOrgIds = orgIds.Except(input.OrgIds);
        if (deleteOrgIds != null && deleteOrgIds.Any())
        {
            await _userOrgRep.DeleteAsync(a => a.UserId == userId && deleteOrgIds.Contains(a.OrgId));
        }

        if (insertOrgIds != null && insertOrgIds.Any())
        {
            var orgs = insertOrgIds.Select(orgId => new UserOrgEntity
            {
                UserId = userId,
                OrgId = orgId
            }).ToList();
            await _userOrgRep.InsertAsync(orgs);
        }

        await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
    }

    /// <summary>
    /// 新增会员
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<long> AddMemberAsync(UserAddMemberInput input)
    {
        if (input.Password.IsNull())
        {
            input.Password = _appConfig.DefaultPassword;
        }
        _userHelper.CheckPassword(input.Password);

        using var _ = _userRep.DataFilter.DisableAll();

        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.Select.Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception(_adminLocalizer["账号已存在"]);
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception(_adminLocalizer["手机号已存在"]);
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception(_adminLocalizer["邮箱已存在"]);
            }
        }

        // 用户信息
        var entity = Mapper.Map<UserEntity>(input);
        entity.Type = UserType.Member;
        if (_appConfig.PasswordHasher)
        {
            entity.Password = _passwordHasher.Value.HashPassword(entity, input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        var user = await _userRep.InsertAsync(entity);

        return user.Id;
    }

    /// <summary>
    /// 修改会员
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateMemberAsync(UserUpdateMemberInput input)
    {
        using var _ = _userRep.DataFilter.DisableAll();

        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.Select.Where(a => a.Id != input.Id).Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception(_adminLocalizer["账号已存在"]);
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception(_adminLocalizer["手机号已存在"]);
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception(_adminLocalizer["邮箱已存在"]);
            }
        }

        var user = await _userRep.GetAsync(input.Id);
        if (!(user?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["用户不存在"]);
        }

        Mapper.Map(input, user);
        await _userRep.UpdateAsync(user);
    }

    /// <summary>
    /// 更新用户基本信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Login]
    public async Task UpdateBasicAsync(UserUpdateBasicInput input)
    {
        var entity = await _userRep.GetAsync(User.Id);
        entity = Mapper.Map(input, entity);
        await _userRep.UpdateAsync(entity);
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
            throw ResultOutput.Exception(_adminLocalizer["新密码和确认密码不一致"]);
        }

        _userHelper.CheckPassword(input.NewPassword);

        var entity = await _userRep.GetAsync(User.Id);
        var oldPassword = MD5Encrypt.Encrypt32(input.OldPassword);
        if (oldPassword != entity.Password)
        {
            throw ResultOutput.Exception(_adminLocalizer["旧密码不正确"]);
        }

        if (entity.PasswordEncryptType == PasswordEncryptType.PasswordHasher)
        {
            entity.Password = _passwordHasher.Value.HashPassword(entity, input.NewPassword);
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(input.NewPassword);
        }
       
        await _userRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<string> ResetPasswordAsync(UserResetPasswordInput input)
    {
        var password = input.Password;
        if (password.IsNull())
        {
            password = _appConfig.DefaultPassword;
        }
        else
        {
            _userHelper.CheckPassword(password);
        }
        if (password.IsNull())
        {
            password = "123asd";
        }

        var entity = await _userRep.GetAsync(input.Id);
        if (entity.PasswordEncryptType == PasswordEncryptType.PasswordHasher)
        {
            entity.Password = _passwordHasher.Value.HashPassword(entity, password);
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(password);
        }
        await _userRep.UpdateAsync(entity);
        return password;
    }

    /// <summary>
    /// 设置主管
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetManagerAsync(UserSetManagerInput input)
    {
        var entity = await _userOrgRep.Where(a => a.UserId == input.UserId && a.OrgId == input.OrgId).FirstAsync();
        entity.IsManager = input.IsManager;
        await _userOrgRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 设置启用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetEnableAsync(UserSetEnableInput input)
    {
        var entity = await _userRep.GetAsync(input.UserId);
        if (entity.Type == UserType.PlatformAdmin)
        {
            throw ResultOutput.Exception(_adminLocalizer["平台管理员禁止禁用"]);
        }
        if (entity.Type == UserType.TenantAdmin)
        {
            throw ResultOutput.Exception(_adminLocalizer["企业管理员禁止禁用"]);
        }
        entity.Enabled = input.Enabled;
        await _userRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var user = await _userRep.Select.WhereDynamic(id).ToOneAsync(a => new { a.Type });
        if (user == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["用户不存在"]);
        }

        if (user.Type == UserType.PlatformAdmin)
        {
            throw ResultOutput.Exception(_adminLocalizer["平台管理员禁止删除"]);
        }

        if (user.Type == UserType.TenantAdmin)
        {
            throw ResultOutput.Exception(_adminLocalizer["企业管理员禁止删除"]);
        }

        //删除用户角色
        await _userRoleRep.DeleteAsync(a => a.UserId == id);
        //删除用户所属部门
        await _userOrgRep.DeleteAsync(a => a.UserId == id);
        //删除员工
        await _userStaffRep.DeleteAsync(a => a.Id == id);
        //删除用户
        await _userRep.DeleteAsync(a => a.Id == id);

        //删除用户数据权限缓存
        await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(id));
    }

    /// <summary>
    /// 批量彻底删除用户
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var admin = await _userRep.Select.Where(a => ids.Contains(a.Id) &&
        (a.Type == UserType.PlatformAdmin || a.Type == UserType.TenantAdmin)).AnyAsync();

        if (admin)
        {
            throw ResultOutput.Exception(_adminLocalizer["平台管理员禁止删除"]);
        }

        //删除用户角色
        await _userRoleRep.DeleteAsync(a => ids.Contains(a.UserId));
        //删除用户所属部门
        await _userOrgRep.DeleteAsync(a => ids.Contains(a.UserId));
        //删除员工
        await _userStaffRep.DeleteAsync(a => ids.Contains(a.Id));
        //删除用户
        await _userRep.DeleteAsync(a => ids.Contains(a.Id));

        foreach (var userId in ids)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
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
        var user = await _userRep.Select.WhereDynamic(id).ToOneAsync(a => new { a.Type });
        if (user == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["用户不存在"]);
        }

        if (user.Type == UserType.PlatformAdmin || user.Type == UserType.TenantAdmin)
        {
            throw ResultOutput.Exception(_adminLocalizer["平台管理员禁止删除"]);
        }

        await _userRoleRep.DeleteAsync(a => a.UserId == id);
        await _userOrgRep.DeleteAsync(a => a.UserId == id);
        await _userStaffRep.SoftDeleteAsync(id);
        await _userRep.SoftDeleteAsync(id);

        await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(id));
    }

    /// <summary>
    /// 批量删除用户
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var admin = await _userRep.Select.Where(a => ids.Contains(a.Id) &&
        (a.Type == UserType.PlatformAdmin || a.Type == UserType.TenantAdmin)).AnyAsync();

        if (admin)
        {
            throw ResultOutput.Exception(_adminLocalizer["平台管理员禁止删除"]);
        }

        await _userRoleRep.DeleteAsync(a => ids.Contains(a.UserId));
        await _userOrgRep.DeleteAsync(a => ids.Contains(a.UserId));
        await _userStaffRep.SoftDeleteAsync(ids);
        await _userRep.SoftDeleteAsync(ids);

        foreach (var userId in ids)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
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
    public async Task<string> AvatarUpload(IFormFile file, bool autoUpdate = false)
    {
        var fileInfo = await _fileService.Value.UploadFileAsync(file);
        if (autoUpdate)
        {
            var entity = await _userRep.GetAsync(User.Id);
            entity.Avatar = fileInfo.LinkUrl;
            await _userRep.UpdateAsync(entity);
        }
        return fileInfo.LinkUrl;
    }

    /// <summary>
    /// 一键登录用户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> OneClickLoginAsync([Required] string userName)
    {
        if (userName.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["请选择用户"]);
        }

        var userRep = _userRep;
        using var _ = userRep.DataFilter.DisableAll();
        using var __ = userRep.DataFilter.Enable(FilterNames.Tenant);

        var user = await userRep.Select.Where(a => a.UserName == userName).ToOneAsync();

        if (user == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["用户不存在"]);
        }

        var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);
        if (_appConfig.Tenant)
        {
            var tenant = await _tenantRep.Value.Select.WhereDynamic(user.TenantId).ToOneAsync<AuthLoginTenantDto>();
            authLoginOutput.Tenant = tenant;
        }

        string token = AppInfo.GetRequiredService<IAuthService>().GetToken(authLoginOutput);

        return new { token };
    }
}