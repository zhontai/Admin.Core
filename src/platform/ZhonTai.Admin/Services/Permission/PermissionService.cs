using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.Permission.Dto;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Domain.TenantPermission;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Domain.PermissionApi;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.User;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Core.Consts;
using FreeSql;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.PkgPermission;
using ZhonTai.Admin.Domain.TenantPkg;

namespace ZhonTai.Admin.Services.Permission;

/// <summary>
/// 权限服务
/// </summary>
[Order(40)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class PermissionService : BaseService, IPermissionService, IDynamicApi
{
    private AppConfig _appConfig => LazyGetRequiredService<AppConfig>();
    private IPermissionRepository _permissionRepository => LazyGetRequiredService<IPermissionRepository>();
    private IRoleRepository _roleRepository => LazyGetRequiredService<IRoleRepository>();
    private IUserRepository _userRepository => LazyGetRequiredService<IUserRepository>();
    private IRolePermissionRepository _rolePermissionRepository => LazyGetRequiredService<IRolePermissionRepository>();
    private ITenantPermissionRepository _tenantPermissionRepository => LazyGetRequiredService<ITenantPermissionRepository>();
    private IUserRoleRepository _userRoleRepository => LazyGetRequiredService<IUserRoleRepository>();
    private IPermissionApiRepository _permissionApiRepository => LazyGetRequiredService<IPermissionApiRepository>();

    public PermissionService()
    {
    }

    /// <summary>
    /// 清除权限下关联的用户权限缓存
    /// </summary>
    /// <param name="permissionIds"></param>
    /// <returns></returns>
    private async Task ClearUserPermissionsAsync(List<long> permissionIds)
    {
        var userIds = await _userRoleRepository.Select.Where(a =>
            _rolePermissionRepository
            .Where(b => b.RoleId == a.RoleId && permissionIds.Contains(b.PermissionId))
            .Any()
        ).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.UserPermissions + userId);
        }
    }

    /// <summary>
    /// 查询分组
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetGroupOutput> GetGroupAsync(long id)
    {
        var result = await _permissionRepository.GetAsync<PermissionGetGroupOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询菜单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetMenuOutput> GetMenuAsync(long id)
    {
        var result = await _permissionRepository.GetAsync<PermissionGetMenuOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询接口
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetApiOutput> GetApiAsync(long id)
    {
        var result = await _permissionRepository.GetAsync<PermissionGetApiOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询权限点
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetDotOutput> GetDotAsync(long id)
    {
        var output = await _permissionRepository.Select
        .WhereDynamic(id)
        .ToOneAsync(a => new PermissionGetDotOutput
        {
            ApiIds = _permissionApiRepository.Where(b => b.PermissionId == a.Id).OrderBy(a => a.Id).ToList(b => b.Api.Id)
        });
        return output;
    }

    /// <summary>
    /// 查询权限列表
    /// </summary>
    /// <param name="key"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public async Task<List<PermissionListOutput>> GetListAsync(string key, DateTime? start, DateTime? end)
    {
        if (end.HasValue)
        {
            end = end.Value.AddDays(1);
        }

        var data = await _permissionRepository
            .WhereIf(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
            .WhereIf(start.HasValue && end.HasValue, a => a.CreatedTime.Value.BetweenEnd(start.Value, end.Value))
            .Include(a => a.View)
            .OrderBy(a => new { a.ParentId, a.Sort })
            .ToListAsync(a=> new PermissionListOutput 
            {
                ViewPath = a.View.Path,
                ApiPaths = string.Join(";", _permissionApiRepository.Where(b=>b.PermissionId == a.Id).ToList(b => b.Api.Path)) 
            });

        return data;
    }

    /// <summary>
    /// 查询授权权限列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<dynamic>> GetPermissionListAsync()
    {
        var permissions = await _permissionRepository.Select
            .Where(a => a.Enabled == true)
            .WhereIf(_appConfig.Tenant && User.TenantType == TenantType.Tenant, a =>
                _tenantPermissionRepository
                .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                .Any()

                ||

                _permissionRepository.Orm.Select<TenantPkgEntity, PkgPermissionEntity>()
                .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                .Any()
            )
            .AsTreeCte(up: true)
            .ToListAsync(a => new { a.Id, a.ParentId, a.Label, a.Type, a.Sort });

        var menus = permissions.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort)
            .Select(a => new
            {
                a.Id,
                a.ParentId,
                a.Label,
                Row = a.Type == PermissionType.Menu
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
        var permissionIds = await _rolePermissionRepository
            .Select.Where(d => d.RoleId == roleId)
            .ToListAsync(a => a.PermissionId);

        return permissionIds;
    }

    /// <summary>
    /// 查询租户权限列表
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [Obsolete("请使用查询套餐权限列表PkgService.GetPkgPermissionListAsync")]
    public async Task<List<long>> GetTenantPermissionListAsync(long tenantId)
    {
        var permissionIds = await _tenantPermissionRepository
            .Select.Where(d => d.TenantId == tenantId)
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
        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = PermissionType.Group;

        if (entity.Sort == 0)
        {
            var sort = await _permissionRepository.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _permissionRepository.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddMenuAsync(PermissionAddMenuInput input)
    {
        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = PermissionType.Menu;
        if (entity.Sort == 0)
        {
            var sort = await _permissionRepository.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _permissionRepository.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 新增接口
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddApiAsync(PermissionAddApiInput input)
    {
        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = PermissionType.Dot;
        if (entity.Sort == 0)
        {
            var sort = await _permissionRepository.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _permissionRepository.InsertAsync(entity);

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
        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = PermissionType.Dot;
        if (entity.Sort == 0)
        {
            var sort = await _permissionRepository.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _permissionRepository.InsertAsync(entity);

        if (input.ApiIds != null && input.ApiIds.Any())
        {
            var permissionApis = input.ApiIds.Select(a => new PermissionApiEntity { PermissionId = entity.Id, ApiId = a }).ToList();
            await _permissionApiRepository.InsertAsync(permissionApis);
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
        var entity = await _permissionRepository.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        await _permissionRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateMenuAsync(PermissionUpdateMenuInput input)
    {
        var entity = await _permissionRepository.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        await _permissionRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改接口
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateApiAsync(PermissionUpdateApiInput input)
    {
        var entity = await _permissionRepository.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        await _permissionRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改权限点
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateDotAsync(PermissionUpdateDotInput input)
    {
        var entity = await _permissionRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("权限点不存在！");
        }

        Mapper.Map(input, entity);
        await _permissionRepository.UpdateAsync(entity);
        await _permissionApiRepository.DeleteAsync(a => a.PermissionId == entity.Id);

        if (input.ApiIds != null && input.ApiIds.Any())
        {
            var permissionApis = input.ApiIds.Select(a => new PermissionApiEntity { PermissionId = entity.Id, ApiId = a });
            await _permissionApiRepository.InsertAsync(permissionApis.ToList());
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
        var ids = _permissionRepository.Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToList(a => a.Id);

        //删除权限关联接口
        await _permissionApiRepository.DeleteAsync(a => ids.Contains(a.PermissionId));

        //删除相关权限
        await _permissionRepository.DeleteAsync(a => ids.Contains(a.Id));

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
        var ids = _permissionRepository.Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToList(a => a.Id);

        //删除权限
        await _permissionRepository.SoftDeleteAsync(a => ids.Contains(a.Id));

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
        var exists = await _roleRepository.Select.DisableGlobalFilter(FilterNames.Tenant).WhereDynamic(input.RoleId).AnyAsync();
        if (!exists)
        {
            throw ResultOutput.Exception("该角色不存在或已被删除！");
        }

        //查询角色权限
        var permissionIds = await _rolePermissionRepository.Select.Where(d => d.RoleId == input.RoleId).ToListAsync(m => m.PermissionId);

        //批量删除权限
        var deleteIds = permissionIds.Where(d => !input.PermissionIds.Contains(d));
        if (deleteIds.Any())
        {
            await _rolePermissionRepository.DeleteAsync(m => m.RoleId == input.RoleId && deleteIds.Contains(m.PermissionId));
        }

        //批量插入权限
        var insertRolePermissions = new List<RolePermissionEntity>();
        var insertPermissionIds = input.PermissionIds.Where(d => !permissionIds.Contains(d));

        //防止租户非法授权，查询主库租户权限范围
        if (_appConfig.Tenant && User.TenantType == TenantType.Tenant)
        {
            var cloud = ServiceProvider.GetRequiredService<FreeSqlCloud>();
            var mainDb = cloud.Use(DbKeys.AppDb);
            var tenantPermissionIds = await mainDb.Select<TenantPermissionEntity>()
                .Where(a => a.TenantId == User.TenantId).ToListAsync(a => a.PermissionId);

            var pkgPermissionIds = await mainDb.Select<PkgPermissionEntity>()
                .Where(a => 
                    mainDb.Select<TenantPkgEntity>()
                    .Where((b) => b.PkgId == a.PkgId && b.TenantId == User.TenantId)
                    .Any()
                )
                .ToListAsync(a => a.PermissionId);

            insertPermissionIds = insertPermissionIds.Where(d => tenantPermissionIds.Contains(d) || pkgPermissionIds.Contains(d));
        }

        if (insertPermissionIds.Any())
        {
            foreach (var permissionId in insertPermissionIds)
            {
                insertRolePermissions.Add(new RolePermissionEntity()
                {
                    RoleId = input.RoleId,
                    PermissionId = permissionId,
                });
            }
            await _rolePermissionRepository.InsertAsync(insertRolePermissions);
        }

        //清除角色下关联的用户权限缓存
        var userIds = await _userRoleRepository.Select.Where(a => a.RoleId == input.RoleId).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.UserPermissions + userId);
        }
    }

    /// <summary>
    /// 保存租户权限
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    [Obsolete("请使用设置套餐权限PkgService.SetPkgPermissionsAsync")]
    public virtual async Task SaveTenantPermissionsAsync(PermissionSaveTenantPermissionsInput input)
    {
        //查询租户权限
        var permissionIds = await _tenantPermissionRepository.Select.Where(d => d.TenantId == input.TenantId).ToListAsync(m => m.PermissionId);

        //批量删除租户权限
        var deleteIds = permissionIds.Where(d => !input.PermissionIds.Contains(d));
        if (deleteIds.Any())
        {
            await _tenantPermissionRepository.DeleteAsync(m => m.TenantId == input.TenantId && deleteIds.Contains(m.PermissionId));
            //删除租户下关联的角色权限
            await _rolePermissionRepository.DeleteAsync(a => deleteIds.Contains(a.PermissionId));
        }

        //批量插入租户权限
        var tenatPermissions = new List<TenantPermissionEntity>();
        var insertPermissionIds = input.PermissionIds.Where(d => !permissionIds.Contains(d));
        if (insertPermissionIds.Any())
        {
            foreach (var permissionId in insertPermissionIds)
            {
                tenatPermissions.Add(new TenantPermissionEntity()
                {
                    TenantId = input.TenantId,
                    PermissionId = permissionId,
                });
            }
            await _tenantPermissionRepository.InsertAsync(tenatPermissions);
        }

        //清除租户下所有用户权限缓存
        using var _ = _userRepository.DataFilter.Disable(FilterNames.Tenant);
        var userIds = await _userRepository.Select.Where(a => a.TenantId == input.TenantId).ToListAsync(a => a.Id);
        if(userIds.Any())
        {
            foreach (var userId in userIds)
            {
                await Cache.DelAsync(CacheKeys.UserPermissions + userId);
            }
        }
    }
}