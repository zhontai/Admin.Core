using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using FreeSql;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Domain.PermissionApi;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.PkgPermission;
using ZhonTai.Admin.Domain.TenantPkg;
using ZhonTai.Admin.Services.Permission.Dto;
using ZhonTai.Admin.Resources;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Admin.Services.Permission;

/// <summary>
/// 权限服务
/// </summary>
[Order(40)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class PermissionService : BaseService, IPermissionService, IDynamicApi
{
    private readonly IPermissionRepository _permissionRep;
    private readonly IPermissionApiRepository _permissionApiRep;
    private readonly AdminLocalizer _adminLocalizer;
    private readonly Lazy<IOptions<AppConfig>> _appConfig;
    private readonly Lazy<IRoleRepository> _roleRep;
    private readonly Lazy<IRolePermissionRepository> _rolePermissionRep;
    private readonly Lazy<IUserRoleRepository> _userRoleRep;
    

    public PermissionService(
        IPermissionRepository permissionRep,
        IPermissionApiRepository permissionApiRep,
        AdminLocalizer adminLocalizer,
        Lazy<IOptions<AppConfig>> appConfig,
        Lazy<IRoleRepository> roleRep,
        Lazy<IRolePermissionRepository> rolePermissionRep,
        Lazy<IUserRoleRepository> userRoleRep
    )
    {
        _permissionRep = permissionRep;
        _permissionApiRep = permissionApiRep;
        _adminLocalizer = adminLocalizer;
        _appConfig = appConfig;
        _roleRep = roleRep;
        _rolePermissionRep = rolePermissionRep;
        _userRoleRep = userRoleRep;
    }

    /// <summary>
    /// 清除权限下关联的用户权限缓存
    /// </summary>
    /// <param name="permissionIds"></param>
    /// <returns></returns>
    private async Task ClearUserPermissionsAsync(List<long> permissionIds)
    {
        var userIds = await _userRoleRep.Value.Select.Where(a =>
            _rolePermissionRep.Value
            .Where(b => b.RoleId == a.RoleId && permissionIds.Contains(b.PermissionId))
            .Any()
        ).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.UserPermission + userId);
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// 查询分组
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetGroupOutput> GetGroupAsync(long id)
    {
        var result = await _permissionRep.GetAsync<PermissionGetGroupOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询菜单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetMenuOutput> GetMenuAsync(long id)
    {
        var result = await _permissionRep.GetAsync<PermissionGetMenuOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询权限点
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetDotOutput> GetDotAsync(long id)
    {
        var output = await _permissionRep.Select
        .WhereDynamic(id)
        .ToOneAsync(a => new PermissionGetDotOutput
        {
            ApiIds = _permissionApiRep.Where(b => b.PermissionId == a.Id).OrderBy(a => a.Id).ToList(b => b.Api.Id)
        });
        return output;
    }

    /// <summary>
    /// 查询权限列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<PermissionGetListOutput>> GetListAsync(PermissionGetListInput input)
    {
        var platform = input?.Platform?.Trim();
        var label = input?.Label?.Trim();
        var path = input?.Path?.Trim();

        var select = _permissionRep.Select;
        if (platform.NotNull())
        {
            Expression<Func<PermissionEntity, bool>> where = null;
            where = where.And(a => a.Platform == platform);
            if (platform.ToLower() == AdminConsts.WebName)
            {
                where = where.Or(a => string.IsNullOrEmpty(a.Platform));
            }
            select = select.Where(where);
        }
        else
        {
            select = select.Where(a => string.IsNullOrEmpty(a.Platform));
        }

        var data = await select
            .WhereIf(label.NotNull(), a => a.Label.Contains(label))
            .WhereIf(path.NotNull(), a => a.Path.Contains(path))
            .Include(a => a.View)
            .OrderBy(a => new { a.ParentId, a.Sort })
            .ToListAsync(a => new PermissionGetListOutput
            {
                ViewPath = a.View.Path,
                ApiPaths = string.Join(";", _permissionApiRep.Where(b => b.PermissionId == a.Id).ToList(b => b.Api.Path))
            });

        return data;
    }

    /// <summary>
    /// 查询授权权限列表
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    public async Task<List<PermissionGetPermissionListOutput>> GetPermissionListAsync(string platform)
    {
        var select = _permissionRep.Select;
        if (platform.NotNull())
        {
            Expression<Func<PermissionEntity, bool>> where = null;
            where = where.And(a => a.Platform == platform);
            if (platform.ToLower() == AdminConsts.WebName)
            {
                where = where.Or(a => string.IsNullOrEmpty(a.Platform));
            }
            select = select.Where(where);
        }
        else
        {
            select = select.Where(a => string.IsNullOrEmpty(a.Platform));
        }

        var permissions = await select
            .Where(a => a.Enabled == true && a.IsSystem == false)
            .WhereIf(_appConfig.Value.Value.Tenant && User.TenantType == TenantType.Tenant, a =>
                _permissionRep.Orm.Select<TenantPkgEntity, PkgPermissionEntity>()
                .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                .Any()
            )
           .AsTreeCte(up: true)
           .Where(a => a.Type != PermissionType.Menu || (a.Type == PermissionType.Menu && a.View.Enabled == true))
           .ToListAsync(a => new { a.Id, a.ParentId, a.Label, a.Type, a.Sort });

        var menus = permissions.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort)
            .Select(a => new PermissionGetPermissionListOutput
            {
                Id = a.Id,
                ParentId = a.ParentId,
                Label = a.Label,
                Row = a.Type == PermissionType.Menu,
            }).ToList();

        menus = menus.ToTree((r, c) =>
        {
            return c.ParentId == 0;
        },
        (r, c) =>
        {
            return r.Id == c.ParentId;
        },
        (r, datalist) =>
        {
            r.Children ??= [];
            r.Children.AddRange(datalist);
        });

        return menus;
    }

    /// <summary>
    /// 查询角色权限列表
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<List<long>> GetRolePermissionListAsync(long roleId = 0)
    {
        var permissionIds = await _rolePermissionRep.Value
            .Select.Where(d => d.RoleId == roleId)
            .ToListAsync(a => a.PermissionId);

        return permissionIds;
    }

    /// <summary>
    /// 新增分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddGroupAsync(PermissionAddGroupInput input)
    {
        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["此分组已存在"]);
        }

        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = PermissionType.Group;

        if (entity.Sort == 0)
        {
            var sort = await _permissionRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _permissionRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddMenuAsync(PermissionAddMenuInput input)
    {
        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["此菜单已存在"]);
        }

        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = PermissionType.Menu;
        if (entity.Sort == 0)
        {
            var sort = await _permissionRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _permissionRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 新增权限点
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task<long> AddDotAsync(PermissionAddDotInput input)
    {
        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["此权限点已存在"]);
        }

        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["此权限点已存在"]);
        }

        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["此权限点编码已存在"]);
        }

        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = PermissionType.Dot;
        if (entity.Sort == 0)
        {
            var sort = await _permissionRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _permissionRep.InsertAsync(entity);

        if (input.ApiIds != null && input.ApiIds.Any())
        {
            var permissionApis = input.ApiIds.Select(a => new PermissionApiEntity { PermissionId = entity.Id, ApiId = a }).ToList();
            await _permissionApiRep.InsertAsync(permissionApis);
        }

        return entity.Id;
    }

    /// <summary>
    /// 修改分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateGroupAsync(PermissionUpdateGroupInput input)
    {
        var entity = await _permissionRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["分组不存在"]);
        }

        if (input.Id == input.ParentId)
        {
            throw ResultOutput.Exception(_adminLocalizer["上级分组不能是本分组"]);
        }

        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Id != input.Id && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["此分组已存在"]);
        }

        entity = Mapper.Map(input, entity);
        await _permissionRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateMenuAsync(PermissionUpdateMenuInput input)
    {
        var entity = await _permissionRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["菜单不存在"]);
        }

        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Id != input.Id && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["此菜单已存在"]);
        }

        entity = Mapper.Map(input, entity);
        await _permissionRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改权限点
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateDotAsync(PermissionUpdateDotInput input)
    {
        var entity = await _permissionRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["权限点不存在"]);
        }

        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Id != input.Id && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["此权限点已存在"]);
        }

        Mapper.Map(input, entity);
        await _permissionRep.UpdateAsync(entity);
        await _permissionApiRep.DeleteAsync(a => a.PermissionId == entity.Id);

        if (input.ApiIds != null && input.ApiIds.Any())
        {
            var permissionApis = input.ApiIds.Select(a => new PermissionApiEntity { PermissionId = entity.Id, ApiId = a });
            await _permissionApiRep.InsertAsync(permissionApis.ToList());
        }

        //清除用户权限缓存
        await ClearUserPermissionsAsync(new List<long> { entity.Id });
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        //递归查询所有权限点
        var ids = _permissionRep.Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToList(a => a.Id);

        //删除权限关联接口
        await _permissionApiRep.DeleteAsync(a => ids.Contains(a.PermissionId));

        //删除相关权限
        await _permissionRep.DeleteAsync(a => ids.Contains(a.Id));

        //清除用户权限缓存
        await ClearUserPermissionsAsync(ids);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        //递归查询所有权限点
        var ids = _permissionRep.Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToList(a => a.Id);

        //删除权限
        await _permissionRep.SoftDeleteAsync(a => ids.Contains(a.Id));

        //清除用户权限缓存
        await ClearUserPermissionsAsync(ids);
    }

    /// <summary>
    /// 保存角色权限
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task AssignAsync(PermissionAssignInput input)
    {
        //分配权限的时候判断角色是否存在
        var exists = await _roleRep.Value.Select.DisableGlobalFilter(FilterNames.Tenant).WhereDynamic(input.RoleId).AnyAsync();
        if (!exists)
        {
            throw ResultOutput.Exception(_adminLocalizer["该角色不存在或已被删除"]);
        }

        var platform = input?.Platform;
        Expression<Func<RolePermissionEntity, bool>> where = null;
        if (platform.NotNull())
        {
            where = where.And(a => a.Platform == platform);
            if (platform.ToLower() == AdminConsts.WebName)
            {
                where = where.Or(a => string.IsNullOrEmpty(a.Platform));
            }
        }
        else
        {
            where = where.And(a => string.IsNullOrEmpty(a.Platform));
        }

        //查询角色权限
        var permissionIds = await _rolePermissionRep.Value.Where(where).Where(a => a.RoleId == input.RoleId).ToListAsync(a => a.PermissionId);

        //批量删除权限
        var deleteIds = permissionIds.Where(a => !input.PermissionIds.Contains(a));
        if (deleteIds.Any())
        {
            await _rolePermissionRep.Value.Where(where).Where(a => a.RoleId == input.RoleId && deleteIds.Contains(a.PermissionId)).ToDelete().ExecuteAffrowsAsync();
        }

        //批量插入权限
        var insertRolePermissions = new List<RolePermissionEntity>();
        var insertPermissionIds = input.PermissionIds.Where(a => !permissionIds.Contains(a));

        //防止租户非法授权，查询主库租户权限范围
        if (_appConfig.Value.Value.Tenant && User.TenantType == TenantType.Tenant)
        {
            var cloud = ServiceProvider.GetRequiredService<FreeSqlCloud>();
            var mainDb = cloud.Use(DbKeys.AdminDb);
            var pkgPermissionIds = await mainDb.Select<PkgPermissionEntity>()
                .Where(a => 
                    mainDb.Select<TenantPkgEntity>()
                    .Where((b) => b.PkgId == a.PkgId && b.TenantId == User.TenantId)
                    .Any()
                )
                .ToListAsync(a => a.PermissionId);

            insertPermissionIds = insertPermissionIds.Where(a => pkgPermissionIds.Contains(a));
        }

        if (insertPermissionIds.Any())
        {
            foreach (var permissionId in insertPermissionIds)
            {
                insertRolePermissions.Add(new RolePermissionEntity()
                {
                    Platform = platform,
                    RoleId = input.RoleId,
                    PermissionId = permissionId,
                });
            }
            await _rolePermissionRep.Value.InsertAsync(insertRolePermissions);
        }

        //清除角色下关联的用户权限缓存
        var userIds = await _userRoleRep.Value.Select.Where(a => a.RoleId == input.RoleId).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.UserPermission + userId);
        }
    }
}