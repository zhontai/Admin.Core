using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Domain.DictionaryType;
using ZhonTai.Admin.Domain.Dictionary;
using ZhonTai.Admin.Domain.DictionaryType.Dto;
using ZhonTai.Admin.Domain.Dictionary.Dto;
using ZhonTai.Admin.Domain.Api;
using ZhonTai.Admin.Domain.Api.Dto;
using ZhonTai.Admin.Domain.View.Dto;
using ZhonTai.Admin.Domain.Permission.Dto;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.User.Dto;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.Role.Dto;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.TenantPermission;
using ZhonTai.Admin.Domain.PermissionApi;
using ZhonTai.Admin.Domain.View;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Common.Extensions;
using ZhonTai.Admin.Domain.Organization.Output;
using ZhonTai.Admin.Domain.Position;
using ZhonTai.Admin.Domain.Employee;
using ZhonTai.Admin.Domain.Organization;
using ZhonTai.Admin.Domain.Position.Output;
using ZhonTai.Admin.Domain.Employee.Output;

namespace ZhonTai.Admin.Repositories
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
            var apiTree = apis.Clone().ToTree((r, c) =>
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
            var viewTree = views.Clone().ToTree((r, c) =>
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
            var permissionTree = permissions.Clone().ToTree((r, c) =>
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

            //人事
            #region 部门

            var organizations = await db.Queryable<OrganizationEntity>().ToListAsync<OrganizationDataOutput>();
            var organizationTree = organizations.Clone().ToTree((r, c) =>
            {
                return c.ParentId == 0;
            },
            (r, c) =>
            {
                return r.Id == c.ParentId;
            },
            (r, datalist) =>
            {
                r.Childs ??= new List<OrganizationDataOutput>();
                r.Childs.AddRange(datalist);
            });

            #endregion

            #region 岗位

            var positions = await db.Queryable<PositionEntity>().ToListAsync<PositionDataOutput>();

            #endregion

            #region 员工

            var employees = await db.Queryable<EmployeeEntity>().ToListAsync<EmployeeDataOutput>();

            #endregion

            #endregion

            #endregion

            #region 生成数据

            var isTenant = appConfig.Tenant;

            SaveDataToJsonFile<DictionaryEntity>(dictionaries, isTenant);
            SaveDataToJsonFile<DictionaryTypeEntity>(dictionaryTypes, isTenant);
            SaveDataToJsonFile<UserEntity>(users, isTenant);
            SaveDataToJsonFile<RoleEntity>(roles, isTenant);
            SaveDataToJsonFile<OrganizationEntity>(organizationTree, isTenant);
            SaveDataToJsonFile<PositionEntity>(positions, isTenant);
            SaveDataToJsonFile<EmployeeEntity>(employees, isTenant);
            if (isTenant)
            {
                var tenantId = tenants.Where(a => a.Code.ToLower() == "zhontai").FirstOrDefault()?.Id;
                SaveDataToJsonFile<DictionaryEntity>(dictionaries.Where(a => a.TenantId == tenantId));
                SaveDataToJsonFile<DictionaryTypeEntity>(dictionaryTypes.Where(a => a.TenantId == tenantId));
                SaveDataToJsonFile<UserEntity>(users.Where(a => a.TenantId == tenantId), false);
                SaveDataToJsonFile<RoleEntity>(roles.Where(a => a.TenantId == tenantId));
                organizationTree = organizations.Clone().Where(a => a.TenantId == tenantId).ToList().ToTree((r, c) =>
                {
                    return c.ParentId == 0;
                },
                (r, c) =>
                {
                    return r.Id == c.ParentId;
                },
                (r, datalist) =>
                {
                    r.Childs ??= new List<OrganizationDataOutput>();
                    r.Childs.AddRange(datalist);
                });
                SaveDataToJsonFile<OrganizationEntity>(organizationTree);
                SaveDataToJsonFile<PositionEntity>(positions.Where(a => a.TenantId == tenantId));
                SaveDataToJsonFile<EmployeeEntity>(employees.Where(a => a.TenantId == tenantId));
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
