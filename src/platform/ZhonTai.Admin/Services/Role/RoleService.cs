using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Services.Role.Dto;
using ZhonTai.Admin.Domain.Role.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Domain.RoleOrg;
using System.Collections.Generic;

namespace ZhonTai.Admin.Services.Role;

/// <summary>
/// 角色服务
/// </summary>
[Order(20)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class RoleService : BaseService, IRoleService, IDynamicApi
{
    private IRoleRepository _roleRepository => LazyGetRequiredService<IRoleRepository>();
    private IUserRepository _userRepository => LazyGetRequiredService<IUserRepository>();
    private IUserRoleRepository _userRoleRepository => LazyGetRequiredService<IUserRoleRepository>();
    private IRolePermissionRepository _rolePermissionRepository => LazyGetRequiredService<IRolePermissionRepository>();
    private IRoleOrgRepository _roleOrgRepository => LazyGetRequiredService<IRoleOrgRepository>();

    public RoleService()
    {
    }

    /// <summary>
    /// 添加角色部门
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="orgIds"></param>
    /// <returns></returns>
    private async Task AddRoleOrgAsync(long roleId, long[] orgIds)
    {
        if (orgIds != null && orgIds.Any())
        {
            var roleOrgs = orgIds.Select(orgId => new RoleOrgEntity
            {
                RoleId = roleId,
                OrgId = orgId
            }).ToList();
            await _roleOrgRepository.InsertAsync(roleOrgs);
        }
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<RoleGetOutput> GetAsync(long id)
    {
        var roleEntity = await _roleRepository.Select
        .WhereDynamic(id)
        .IncludeMany(a => a.Orgs.Select(b => new OrgEntity { Id = b.Id }))
        .ToOneAsync(a => new RoleGetOutput { Orgs = a.Orgs });

        var output = Mapper.Map<RoleGetOutput>(roleEntity);

        return output;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<RoleGetListOutput>> GetListAsync([FromQuery]RoleGetListInput input)
    {
        var list = await _roleRepository.Select
        .WhereIf(input.Name.NotNull(), a => a.Name.Contains(input.Name))
        .OrderBy(a => new {a.ParentId, a.Sort})
        .ToListAsync<RoleGetListOutput>();

        return list;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<RoleGetPageOutput>> GetPageAsync(PageInput<RoleGetPageDto> input)
    {
        var key = input.Filter?.Name;

        var list = await _roleRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<RoleGetPageOutput>();

        var data = new PageOutput<RoleGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 查询角色用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<RoleGetRoleUserListOutput>> GetRoleUserListAsync([FromQuery] RoleGetRoleUserListInput input)
    {
        var list = await _userRepository.Select.From<UserRoleEntity>()
            .InnerJoin(a => a.t2.UserId == a.t1.Id)
            .Where(a => a.t2.RoleId == input.RoleId)
            .WhereIf(input.Name.NotNull(), a => a.t1.Name.Contains(input.Name))
            .OrderByDescending(a => a.t1.Id)
            .ToListAsync<RoleGetRoleUserListOutput>();

        return list;
    }

    /// <summary>
    /// 添加角色用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task AddRoleUserAsync(RoleAddRoleUserListInput input)
    {
        var roleId = input.RoleId;
        var userIds = await _userRoleRepository.Select.Where(a => a.RoleId == roleId).ToListAsync(a => a.UserId);
        var insertUserIds = input.UserIds.Except(userIds);
        if (insertUserIds != null && insertUserIds.Any())
        {
            var userRoleList = insertUserIds.Select(userId => new UserRoleEntity 
            { 
                UserId = userId, 
                RoleId = roleId 
            }).ToList();
            await _userRoleRepository.InsertAsync(userRoleList);
        }

        var clearUserIds = userIds.Concat(input.UserIds).Distinct();
        foreach (var userId in clearUserIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 移除角色用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task RemoveRoleUserAsync(RoleAddRoleUserListInput input)
    {
        var userIds = input.UserIds;
        if (userIds != null && userIds.Any())
        {
            await _userRoleRepository.Where(a => a.RoleId == input.RoleId && input.UserIds.Contains(a.UserId)).ToDelete().ExecuteAffrowsAsync();
        }

        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(RoleAddInput input)
    {
        if (await _roleRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"此{(input.Type == RoleType.Group ? "分组" : "角色")}已存在");
        }

        if (input.Code.NotNull() && await _roleRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"此{(input.Type == RoleType.Group ? "分组" : "角色")}编码已存在");
        }

        var entity = Mapper.Map<RoleEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _roleRepository.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _roleRepository.InsertAsync(entity);
        if (input.DataScope == DataScope.Custom)
        {
            await AddRoleOrgAsync(entity.Id, input.OrgIds);
        }

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(RoleUpdateInput input)
    {
        var entity = await _roleRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("角色不存在");
        }

        if (await _roleRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"此{(input.Type == RoleType.Group ? "分组" : "角色")}已存在");
        }

        if (input.Code.NotNull() && await _roleRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"此{(input.Type == RoleType.Group ? "分组" : "角色")}编码已存在");
        }

        Mapper.Map(input, entity);
        await _roleRepository.UpdateAsync(entity);
        await _roleOrgRepository.DeleteAsync(a => a.RoleId == entity.Id);
        if (input.DataScope == DataScope.Custom)
        {
            await AddRoleOrgAsync(entity.Id, input.OrgIds);
        }

        var userIds = await _userRoleRepository.Select.Where(a => a.RoleId == entity.Id).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }    

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var roleIdList = await _roleRepository.GetChildIdListAsync(id);
        var userIds = await _userRoleRepository.Select.Where(a => roleIdList.Contains(a.RoleId)).ToListAsync(a => a.UserId);

        //删除用户角色
        await _userRoleRepository.DeleteAsync(a => a.UserId == id);
        //删除角色权限
        await _rolePermissionRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        //删除角色
        await _roleRepository.DeleteAsync(a => roleIdList.Contains(a.Id));
        
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var roleIdList = await _roleRepository.GetChildIdListAsync(ids);
        var userIds = await _userRoleRepository.Select.Where(a => roleIdList.Contains(a.RoleId)).ToListAsync(a => a.UserId);

        //删除用户角色
        await _userRoleRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        //删除角色权限
        await _rolePermissionRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        //删除角色
        await _roleRepository.Where(a => roleIdList.Contains(a.Id)).AsTreeCte().ToDelete().ExecuteAffrowsAsync();

        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        var roleIdList = await _roleRepository.GetChildIdListAsync(id);
        var userIds = await _userRoleRepository.Select.Where(a => roleIdList.Contains(a.RoleId)).ToListAsync(a => a.UserId);
        await _userRoleRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _rolePermissionRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _roleRepository.SoftDeleteRecursiveAsync(a => roleIdList.Contains(a.Id));
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var roleIdList = await _roleRepository.GetChildIdListAsync(ids);
        var userIds = await _userRoleRepository.Select.Where(a => ids.Contains(a.RoleId)).ToListAsync(a => a.UserId);
        await _userRoleRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _rolePermissionRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _roleRepository.SoftDeleteRecursiveAsync(a => roleIdList.Contains(a.Id));
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 设置数据权限
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetDataScopeAsync(RoleSetDataScopeInput input)
    {
        var entity = await _roleRepository.GetAsync(input.RoleId);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("角色不存在");
        }

        Mapper.Map(input, entity);
        await _roleRepository.UpdateAsync(entity);
        await _roleOrgRepository.DeleteAsync(a => a.RoleId == entity.Id);
        if (input.DataScope == DataScope.Custom)
        {
            await AddRoleOrgAsync(entity.Id, input.OrgIds);
        }

        var userIds = await _userRoleRepository.Select.Where(a => a.RoleId == entity.Id).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }
}