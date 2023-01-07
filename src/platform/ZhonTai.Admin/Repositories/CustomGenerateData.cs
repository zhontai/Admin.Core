using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Domain.DictionaryType;
using ZhonTai.Admin.Domain.Dictionary;
using ZhonTai.Admin.Domain.Api;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.TenantPermission;
using ZhonTai.Admin.Domain.PermissionApi;
using ZhonTai.Admin.Domain.View;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Common.Extensions;
using ZhonTai.Admin.Domain.UserStaff;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Core.Db.Data;
using FreeSql;
using ZhonTai.Admin.Domain.UserOrg;

namespace ZhonTai.Admin.Repositories;

public class CustomGenerateData : GenerateData, IGenerateData
{
    public virtual async Task GenerateDataAsync(IFreeSql db, AppConfig appConfig)
    {
        #region 读取数据

        //admin
        #region 数据字典

        var dictionaryTypes = db.Queryable<DictionaryTypeEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName,
            a.ModifiedTime,
            a.ModifiedUserId,
            a.ModifiedUserName
        });

        var dictionaries = db.Queryable<DictionaryEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName,
            a.ModifiedTime,
            a.ModifiedUserId,
            a.ModifiedUserName
        });
        #endregion

        #region 接口

        var apis = db.Queryable<ApiEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName,
            a.ModifiedTime,
            a.ModifiedUserId,
            a.ModifiedUserName
        });
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
            r.Childs ??= new List<ApiEntity>();
            r.Childs.AddRange(datalist);
        });

        #endregion

        #region 视图

        var views = db.Queryable<ViewEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName,
            a.ModifiedTime,
            a.ModifiedUserId,
            a.ModifiedUserName
        });
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
           r.Childs ??= new List<ViewEntity>();
           r.Childs.AddRange(datalist);
       });

        #endregion

        #region 权限

        var permissions = db.Queryable<PermissionEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName,
            a.ModifiedTime,
            a.ModifiedUserId,
            a.ModifiedUserName
        });
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
           r.Childs ??= new List<PermissionEntity>();
           r.Childs.AddRange(datalist);
       });

        #endregion

        #region 用户

        var users = db.Queryable<UserEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName,
            a.ModifiedTime,
            a.ModifiedUserId,
            a.ModifiedUserName
        });

        #endregion

        #region 员工

        var staffs = db.Queryable<UserStaffEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName,
            a.ModifiedTime,
            a.ModifiedUserId,
            a.ModifiedUserName
        });

        #endregion

        #region 部门

        var orgs = db.Queryable<OrgEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName,
            a.ModifiedTime,
            a.ModifiedUserId,
            a.ModifiedUserName
        });
        var orgTree = orgs.Clone().ToTree((r, c) =>
        {
            return c.ParentId == 0;
        },
        (r, c) =>
        {
            return r.Id == c.ParentId;
        },
        (r, datalist) =>
        {
            r.Childs ??= new List<OrgEntity>();
            r.Childs.AddRange(datalist);
        });

        #endregion

        #region 角色

        var roles = db.Queryable<RoleEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName,
            a.ModifiedTime,
            a.ModifiedUserId,
            a.ModifiedUserName
        });

        #endregion

        #region 用户角色

        var userRoles = db.Queryable<UserRoleEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName
        });

        #endregion

        #region 用户部门

        var userOrgs = db.Queryable<UserOrgEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName,
            a.ModifiedTime,
            a.ModifiedUserId,
            a.ModifiedUserName
        });

        #endregion

        #region 角色权限

        var rolePermissions = db.Queryable<RolePermissionEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName
        });

        #endregion

        #region 租户

        var tenants = db.Queryable<TenantEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName,
            a.ModifiedTime,
            a.ModifiedUserId,
            a.ModifiedUserName
        });

        #endregion

        #region 租户权限

        var tenantPermissions = db.Queryable<TenantPermissionEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName
        });

        #endregion

        #region 权限接口

        var permissionApis = db.Queryable<PermissionApiEntity>().ToListIgnore(a => new
        {
            a.CreatedTime,
            a.CreatedUserId,
            a.CreatedUserName
        });

        #endregion

        #endregion

        #region 生成数据

        var isTenant = appConfig.Tenant;

        SaveDataToJsonFile<DictionaryEntity>(dictionaries, isTenant);
        SaveDataToJsonFile<DictionaryTypeEntity>(dictionaryTypes, isTenant);
        SaveDataToJsonFile<UserEntity>(users, isTenant);
        SaveDataToJsonFile<RoleEntity>(roles, isTenant);
        SaveDataToJsonFile<OrgEntity>(orgTree, isTenant);
        SaveDataToJsonFile<UserStaffEntity>(staffs, isTenant);
        if (isTenant)
        {
            var tenantIds = tenants?.Select(a => a.Id)?.ToList();
            SaveDataToJsonFile<DictionaryEntity>(dictionaries.Where(a => tenantIds.Contains(a.TenantId.Value)));
            SaveDataToJsonFile<DictionaryTypeEntity>(dictionaryTypes.Where(a => tenantIds.Contains(a.TenantId.Value)));
            SaveDataToJsonFile<UserEntity>(users.Where(a => tenantIds.Contains(a.TenantId.Value)), false);
            SaveDataToJsonFile<RoleEntity>(roles.Where(a => tenantIds.Contains(a.TenantId.Value)));
            orgTree = orgs.Clone().Where(a => tenantIds.Contains(a.TenantId.Value)).ToList().ToTree((r, c) =>
            {
                return c.ParentId == 0;
            },
            (r, c) =>
            {
                return r.Id == c.ParentId;
            },
            (r, datalist) =>
            {
                r.Childs ??= new List<OrgEntity>();
                r.Childs.AddRange(datalist);
            });
            SaveDataToJsonFile<OrgEntity>(orgTree);
            SaveDataToJsonFile<UserStaffEntity>(staffs.Where(a => tenantIds.Contains(a.TenantId.Value)));
        }
        SaveDataToJsonFile<UserRoleEntity>(userRoles);
        SaveDataToJsonFile<UserOrgEntity>(userOrgs);
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
