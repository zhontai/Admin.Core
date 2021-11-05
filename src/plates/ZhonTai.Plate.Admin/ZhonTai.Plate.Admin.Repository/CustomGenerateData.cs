using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Db;
using ZhonTai.Plate.Admin.Domain.DictionaryType;
using ZhonTai.Plate.Admin.Domain.Dictionary;
using ZhonTai.Plate.Admin.Domain.DictionaryType.Dto;
using ZhonTai.Plate.Admin.Domain.Dictionary.Dto;
using ZhonTai.Plate.Admin.Domain.Api;
using ZhonTai.Plate.Admin.Domain.Api.Dto;
using ZhonTai.Plate.Admin.Domain.View.Dto;
using ZhonTai.Plate.Admin.Domain.Permission.Dto;
using ZhonTai.Plate.Admin.Domain.Permission;
using ZhonTai.Plate.Admin.Domain.User;
using ZhonTai.Plate.Admin.Domain.User.Dto;
using ZhonTai.Plate.Admin.Domain.Role;
using ZhonTai.Plate.Admin.Domain.Role.Dto;
using ZhonTai.Plate.Admin.Domain.UserRole;
using ZhonTai.Plate.Admin.Domain.RolePermission;
using ZhonTai.Plate.Admin.Domain.Tenant;
using ZhonTai.Plate.Admin.Domain.TenantPermission;
using ZhonTai.Plate.Admin.Domain.PermissionApi;
using ZhonTai.Plate.Admin.Domain.View;
using ZhonTai.Common.Configs;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Plate.Admin.Repository
{
    public class CustomGenerateData : GenerateData, IGenerateData
    {
        public virtual async Task GenerateDataAsync(IFreeSql db, AppConfig appConfig)
        {
            #region 读取数据

            //admin
            #region 数据字典

            var dictionaryTypes = await db.Queryable<DictionaryTypeEntity>().ToListAsync<DictionaryTypeDataOutput>();
            var dictionaries = await db.Queryable<DictionaryEntity>().ToListAsync<DictionaryDataOutput>();

            #endregion

            #region 接口

            var apis = await db.Queryable<ApiEntity>().ToListAsync<ApiDataOutput>();
            var apiTree = apis.ToTree((r, c) =>
            {
                return c.ParentId == 0;
            },
            (r, c) =>
            {
                return r.Id == c.ParentId;
            },
            (r, datalist) =>
            {
                r.Childs ??= new List<ApiDataOutput>();
                r.Childs.AddRange(datalist);
            });

            #endregion

            #region 视图

            var views = await db.Queryable<ViewEntity>().ToListAsync<ViewDataOutput>();
            var viewTree = views.ToTree((r, c) =>
            {
                return c.ParentId == 0;
            },
           (r, c) =>
           {
               return r.Id == c.ParentId;
           },
           (r, datalist) =>
           {
               r.Childs ??= new List<ViewDataOutput>();
               r.Childs.AddRange(datalist);
           });

            #endregion

            #region 权限

            var permissions = await db.Queryable<PermissionEntity>().ToListAsync<PermissionDataOutput>();
            var permissionTree = permissions.ToTree((r, c) =>
            {
                return c.ParentId == 0;
            },
           (r, c) =>
           {
               return r.Id == c.ParentId;
           },
           (r, datalist) =>
           {
               r.Childs ??= new List<PermissionDataOutput>();
               r.Childs.AddRange(datalist);
           });

            #endregion

            #region 用户

            var users = await db.Queryable<UserEntity>().ToListAsync<UserDataOutput>();

            #endregion

            #region 角色

            var roles = await db.Queryable<RoleEntity>().ToListAsync<RoleDataOutput>();

            #endregion

            #region 用户角色

            var userRoles = await db.Queryable<UserRoleEntity>().ToListAsync(a => new
            {
                a.Id,
                a.UserId,
                a.RoleId
            });

            #endregion

            #region 角色权限

            var rolePermissions = await db.Queryable<RolePermissionEntity>().ToListAsync(a => new
            {
                a.Id,
                a.RoleId,
                a.PermissionId
            });

            #endregion

            #region 租户

            var tenants = await db.Queryable<TenantEntity>().ToListAsync(a => new
            {
                a.Id,
                a.UserId,
                a.RoleId,
                a.Name,
                a.Code,
                a.RealName,
                a.Phone,
                a.Email,
                a.TenantType,
                a.DataIsolationType,
                a.DbType,
                a.ConnectionString,
                a.IdleTime,
                a.Description
            });

            #endregion

            #region 租户权限

            var tenantPermissions = await db.Queryable<TenantPermissionEntity>().ToListAsync(a => new
            {
                a.Id,
                a.TenantId,
                a.PermissionId
            });

            #endregion

            #region 权限接口

            var permissionApis = await db.Queryable<PermissionApiEntity>().ToListAsync(a => new
            {
                a.Id,
                a.PermissionId,
                a.ApiId
            });

            #endregion

            #endregion

            #region 生成数据

            var isTenant = appConfig.Tenant;

            SaveDataToJsonFile<DictionaryEntity>(dictionaries, isTenant);
            SaveDataToJsonFile<DictionaryTypeEntity>(dictionaryTypes, isTenant);
            SaveDataToJsonFile<UserEntity>(users, isTenant);
            SaveDataToJsonFile<RoleEntity>(roles, isTenant);
            if (isTenant)
            {
                var tenantId = tenants.Where(a => a.Code.ToLower() == "zhontai").FirstOrDefault()?.Id;
                SaveDataToJsonFile<DictionaryEntity>(dictionaries.Where(a => a.TenantId == tenantId));
                SaveDataToJsonFile<DictionaryTypeEntity>(dictionaryTypes.Where(a => a.TenantId == tenantId));
                SaveDataToJsonFile<UserEntity>(users.Where(a => a.TenantId == tenantId), false);
                SaveDataToJsonFile<RoleEntity>(roles.Where(a => a.TenantId == tenantId));
            }
            SaveDataToJsonFile<UserRoleEntity>(userRoles);
            SaveDataToJsonFile<ApiEntity>(apiTree);
            SaveDataToJsonFile<ViewEntity>(viewTree);
            SaveDataToJsonFile<PermissionEntity>(permissionTree);
            SaveDataToJsonFile<PermissionApiEntity>(permissionApis);
            SaveDataToJsonFile<RolePermissionEntity>(rolePermissions);
            SaveDataToJsonFile<TenantEntity>(tenants);
            SaveDataToJsonFile<TenantPermissionEntity>(tenantPermissions, propsContractResolver: new PropsContractResolver());

            #endregion
        }
    }
}
