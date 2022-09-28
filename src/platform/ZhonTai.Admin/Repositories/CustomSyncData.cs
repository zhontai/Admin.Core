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
using ZhonTai.Admin.Domain.Organization;
using ZhonTai.Admin.Domain.Employee;
using ZhonTai.Admin.Core.Db.Data;
using ZhonTai.Admin.Domain;

namespace ZhonTai.Admin.Repositories;

public class CustomSyncData : SyncData, ISyncData
{
    public virtual async Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null)
    {
        using (var uow = db.CreateUnitOfWork())
        using (var tran = uow.GetOrBeginTransaction())
        {
            var isTenant = appConfig.Tenant;

            var dictionaryTypes = GetData<DictionaryTypeEntity>(isTenant);
            await InitDataAsync(db, uow, tran, dictionaryTypes, dbConfig);

            var dictionaries = GetData<DictionaryEntity>(isTenant);
            await InitDataAsync(db, uow, tran, dictionaries, dbConfig);

            var users = GetData<UserEntity>(isTenant);
            await InitDataAsync(db, uow, tran, users, dbConfig);

            var employees = GetData<EmployeeEntity>(isTenant);
            await InitDataAsync(db, uow, tran, employees, dbConfig);

            var organizations = GetData<OrganizationEntity>(isTenant);
            await InitDataAsync(db, uow, tran, organizations, dbConfig);

            var roles = GetData<RoleEntity>(isTenant);
            await InitDataAsync(db, uow, tran, roles, dbConfig);

            var apiTree = GetData<ApiEntity>();
            await InitDataAsync(db, uow, tran, apiTree, dbConfig);

            var viewTree = GetData<ViewEntity>();
            await InitDataAsync(db, uow, tran, viewTree, dbConfig);

            var permissionTree = GetData<PermissionEntity>();
            await InitDataAsync(db, uow, tran, permissionTree, dbConfig);

            var userRoles = GetData<UserRoleEntity>();
            await InitDataAsync(db, uow, tran, userRoles, dbConfig);

            var userOrgs = GetData<EmployeeOrganizationEntity>();
            await InitDataAsync(db, uow, tran, userOrgs, dbConfig);

            var rolePermissions = GetData<RolePermissionEntity>();
            await InitDataAsync(db, uow, tran, rolePermissions, dbConfig);

            var tenants = GetData<TenantEntity>();
            await InitDataAsync(db, uow, tran, tenants, dbConfig);

            var tenantPermissions = GetData<TenantPermissionEntity>();
            await InitDataAsync(db, uow, tran, tenantPermissions, dbConfig);

            var permissionApis = GetData<PermissionApiEntity>();
            await InitDataAsync(db, uow, tran, permissionApis, dbConfig);

            uow.Commit();
        }
    }
}
