using Admin.Core.Common.Attributes;
using Admin.Core.Common.BaseModel;
using Admin.Core.Common.Cache;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Repository;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.Permission.Input;
using Admin.Core.Service.Admin.Permission.Output;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Core.Service.Admin.Permission
{
    public class PermissionService : BaseService, IPermissionService
    {
        private readonly AppConfig _appConfig;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRepositoryBase<RolePermissionEntity> _rolePermissionRepository;
        private readonly IRepositoryBase<TenantPermissionEntity> _tenantPermissionRepository;
        private readonly IRepositoryBase<UserRoleEntity> _userRoleRepository;
        private readonly IRepositoryBase<PermissionApiEntity> _permissionApiRepository;

        public PermissionService(
            AppConfig appConfig,
            IPermissionRepository permissionRepository,
            IRoleRepository roleRepository,
            IRepositoryBase<RolePermissionEntity> rolePermissionRepository,
            IRepositoryBase<TenantPermissionEntity> tenantPermissionRepository,
            IRepositoryBase<UserRoleEntity> userRoleRepository,
            IRepositoryBase<PermissionApiEntity> permissionApiRepository
        )
        {
            _appConfig = appConfig;
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _tenantPermissionRepository = tenantPermissionRepository;
            _userRoleRepository = userRoleRepository;
            _permissionApiRepository = permissionApiRepository;
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
                await Cache.DelAsync(string.Format(CacheKey.UserPermissions, userId));
            }
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var result = await _permissionRepository.GetAsync(id);

            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> GetGroupAsync(long id)
        {
            var result = await _permissionRepository.GetAsync<PermissionGetGroupOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> GetMenuAsync(long id)
        {
            var result = await _permissionRepository.GetAsync<PermissionGetMenuOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> GetApiAsync(long id)
        {
            var result = await _permissionRepository.GetAsync<PermissionGetApiOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> GetDotAsync(long id)
        {
            var entity = await _permissionRepository.Select
            .WhereDynamic(id)
            .IncludeMany(a => a.Apis.Select(b => new ApiEntity { Id = b.Id }))
            .ToOneAsync();

            var output = Mapper.Map<PermissionGetDotOutput>(entity);

            return ResponseOutput.Ok(output);
        }

        public async Task<IResponseOutput> GetListAsync(string key, DateTime? start, DateTime? end)
        {
            if (end.HasValue)
            {
                end = end.Value.AddDays(1);
            }

            var data = await _permissionRepository
                .WhereIf(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
                .WhereIf(start.HasValue && end.HasValue, a => a.CreatedTime.Value.BetweenEnd(start.Value, end.Value))
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync(a=> new PermissionListOutput { ApiPaths = string.Join(";", _permissionApiRepository.Where(b=>b.PermissionId == a.Id).ToList(b => b.Api.Path)) });

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> AddGroupAsync(PermissionAddGroupInput input)
        {
            var entity = Mapper.Map<PermissionEntity>(input);
            var id = (await _permissionRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Ok(id > 0);
        }

        public async Task<IResponseOutput> AddMenuAsync(PermissionAddMenuInput input)
        {
            var entity = Mapper.Map<PermissionEntity>(input);
            var id = (await _permissionRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Ok(id > 0);
        }

        public async Task<IResponseOutput> AddApiAsync(PermissionAddApiInput input)
        {
            var entity = Mapper.Map<PermissionEntity>(input);
            var id = (await _permissionRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Ok(id > 0);
        }

        [Transaction]
        public async Task<IResponseOutput> AddDotAsync(PermissionAddDotInput input)
        {
            var entity = Mapper.Map<PermissionEntity>(input);
            var id = (await _permissionRepository.InsertAsync(entity)).Id;

            if (input.ApiIds != null && input.ApiIds.Any())
            {
                var permissionApis = input.ApiIds.Select(a => new PermissionApiEntity { PermissionId = id, ApiId = a });
                await _permissionApiRepository.InsertAsync(permissionApis);
            }

            return ResponseOutput.Ok(id > 0);
        }

        public async Task<IResponseOutput> UpdateGroupAsync(PermissionUpdateGroupInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _permissionRepository.GetAsync(input.Id);
                entity = Mapper.Map(input, entity);
                result = (await _permissionRepository.UpdateAsync(entity)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> UpdateMenuAsync(PermissionUpdateMenuInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _permissionRepository.GetAsync(input.Id);
                entity = Mapper.Map(input, entity);
                result = (await _permissionRepository.UpdateAsync(entity)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> UpdateApiAsync(PermissionUpdateApiInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _permissionRepository.GetAsync(input.Id);
                entity = Mapper.Map(input, entity);
                result = (await _permissionRepository.UpdateAsync(entity)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        [Transaction]
        public async Task<IResponseOutput> UpdateDotAsync(PermissionUpdateDotInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _permissionRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("权限点不存在！");
            }

            Mapper.Map(input, entity);
            await _permissionRepository.UpdateAsync(entity);

            await _permissionApiRepository.DeleteAsync(a => a.PermissionId == entity.Id);

            if (input.ApiIds != null && input.ApiIds.Any())
            {
                var permissionApis = input.ApiIds.Select(a => new PermissionApiEntity { PermissionId = entity.Id, ApiId = a });
                await _permissionApiRepository.InsertAsync(permissionApis);
            }

            //清除用户权限缓存
            await ClearUserPermissionsAsync(new List<long> { entity.Id });

            return ResponseOutput.Ok();
        }

        [Transaction]
        public async Task<IResponseOutput> DeleteAsync(long id)
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

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> SoftDeleteAsync(long id)
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

            return ResponseOutput.Ok();
        }

        [Transaction]
        public async Task<IResponseOutput> AssignAsync(PermissionAssignInput input)
        {
            //分配权限的时候判断角色是否存在
            var exists = await _roleRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(input.RoleId).AnyAsync();
            if (!exists)
            {
                return ResponseOutput.NotOk("该角色不存在或已被删除！");
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

            //防止租户非法授权
            if (_appConfig.Tenant && User.TenantType == TenantType.Tenant)
            {
                var masterDb = ServiceProvider.GetRequiredService<IFreeSql>();
                var tenantPermissionIds = await masterDb.GetRepositoryBase<TenantPermissionEntity>().Select.Where(d => d.TenantId == User.TenantId).ToListAsync(m => m.PermissionId);
                insertPermissionIds = insertPermissionIds.Where(d => tenantPermissionIds.Contains(d));
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
                await Cache.DelAsync(string.Format(CacheKey.UserPermissions, userId));
            }

            return ResponseOutput.Ok();
        }

        [Transaction]
        public async Task<IResponseOutput> SaveTenantPermissionsAsync(PermissionSaveTenantPermissionsInput input)
        {
            //获得租户db
            var ib = ServiceProvider.GetRequiredService<IdleBus<IFreeSql>>();
            var tenantDb = ib.GetTenantFreeSql(ServiceProvider, input.TenantId);

            //查询租户权限
            var permissionIds = await _tenantPermissionRepository.Select.Where(d => d.TenantId == input.TenantId).ToListAsync(m => m.PermissionId);

            //批量删除租户权限
            var deleteIds = permissionIds.Where(d => !input.PermissionIds.Contains(d));
            if (deleteIds.Any())
            {
                await _tenantPermissionRepository.DeleteAsync(m => m.TenantId == input.TenantId && deleteIds.Contains(m.PermissionId));
                //删除租户下关联的角色权限
                await tenantDb.GetRepositoryBase<RolePermissionEntity>().DeleteAsync(a => deleteIds.Contains(a.PermissionId));
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
            var userIds = await tenantDb.GetRepositoryBase<UserEntity>().Select.Where(a => a.TenantId == input.TenantId).ToListAsync(a => a.Id);
            if(userIds.Any())
            {
                foreach (var userId in userIds)
                {
                    await Cache.DelAsync(string.Format(CacheKey.UserPermissions, userId));
                }
            }

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> GetPermissionList()
        {
            var permissions = await _permissionRepository.Select
                .WhereIf(_appConfig.Tenant && User.TenantType == TenantType.Tenant, a =>
                    _tenantPermissionRepository
                    .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                    .Any()
                )
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync(a => new { a.Id, a.ParentId, a.Label, a.Type });

            var apis = permissions
                .Where(a => a.Type == PermissionType.Dot)
                .Select(a => new { a.Id, a.ParentId, a.Label });

            var menus = permissions
                .Where(a => (new[] { PermissionType.Group, PermissionType.Menu }).Contains(a.Type))
                .Select(a => new
                {
                    a.Id,
                    a.ParentId,
                    a.Label,
                    Apis = apis.Where(b => b.ParentId == a.Id).Select(b => new { b.Id, b.Label })
                });

            return ResponseOutput.Ok(menus);
        }

        public async Task<IResponseOutput> GetRolePermissionList(long roleId = 0)
        {
            var permissionIds = await _rolePermissionRepository
                .Select.Where(d => d.RoleId == roleId)
                .ToListAsync(a => a.PermissionId);

            return ResponseOutput.Ok(permissionIds);
        }

        public async Task<IResponseOutput> GetTenantPermissionList(long tenantId)
        {
            var permissionIds = await _tenantPermissionRepository
                .Select.Where(d => d.TenantId == tenantId)
                .ToListAsync(a => a.PermissionId);

            return ResponseOutput.Ok(permissionIds);
        }
    }
}