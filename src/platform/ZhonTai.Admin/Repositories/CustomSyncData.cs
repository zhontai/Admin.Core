using System.Threading.Tasks;
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
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Domain.UserStaff;
using ZhonTai.Admin.Core.Db.Data;
using ZhonTai.Admin.Domain.UserOrg;
using System.Linq;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Admin.Repositories;

public class CustomSyncData : SyncData, ISyncData
{
    public virtual async Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null)
    {
        using var uow = db.CreateUnitOfWork();
        using var tran = uow.GetOrBeginTransaction();
        var isTenant = appConfig.Tenant;

        var dictionaryTypes = GetData<DictionaryTypeEntity>(isTenant, dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, dictionaryTypes, dbConfig);

        var dictionaries = GetData<DictionaryEntity>(isTenant, dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, dictionaries, dbConfig);

        var users = GetData<UserEntity>(isTenant, dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, users, dbConfig);

        var staffs = GetData<UserStaffEntity>(isTenant, dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, staffs, dbConfig);

        var orgTree = GetData<OrgEntity>(isTenant, dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, orgTree.ToList().ToPlainList((a) => a.Childs).ToArray(), dbConfig);

        var roles = GetData<RoleEntity>(isTenant, dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, roles, dbConfig);

        var apiTree = GetData<ApiEntity>(path: dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, apiTree.ToList().ToPlainList((a) => a.Childs).ToArray(), dbConfig);

        var viewTree = GetData<ViewEntity>(path: dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, viewTree.ToList().ToPlainList((a) => a.Childs).ToArray(), dbConfig);

        var permissionTree = GetData<PermissionEntity>(path: dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, permissionTree.ToList().ToPlainList((a) => a.Childs).ToArray(), dbConfig);

        var userRoles = GetData<UserRoleEntity>(path: dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, userRoles, dbConfig);

        var userOrgs = GetData<UserOrgEntity>(path: dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, userOrgs, dbConfig);

        var rolePermissions = GetData<RolePermissionEntity>(path: dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, rolePermissions, dbConfig);

        var tenants = GetData<TenantEntity>(path: dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, tenants, dbConfig);

        var tenantPermissions = GetData<TenantPermissionEntity>(path: dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, tenantPermissions, dbConfig);

        var permissionApis = GetData<PermissionApiEntity>(path: dbConfig.SyncDataPath);
        await InitDataAsync(db, uow, tran, permissionApis, dbConfig);

        uow.Commit();
    }
}
