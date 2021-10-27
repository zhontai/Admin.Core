using System.Threading.Tasks;
using ZhonTai.Common.Domain.Db;
using ZhonTai.Plate.Admin.Domain.DictionaryType;
using ZhonTai.Plate.Admin.Domain.Dictionary;
using ZhonTai.Plate.Admin.Domain.Api;
using ZhonTai.Plate.Admin.Domain.Permission;
using ZhonTai.Plate.Admin.Domain.User;
using ZhonTai.Plate.Admin.Domain.Role;
using ZhonTai.Plate.Admin.Domain.UserRole;
using ZhonTai.Plate.Admin.Domain.RolePermission;
using ZhonTai.Plate.Admin.Domain.Tenant;
using ZhonTai.Plate.Admin.Domain.TenantPermission;
using ZhonTai.Plate.Admin.Domain.PermissionApi;
using ZhonTai.Plate.Admin.Domain.View;
using ZhonTai.Common.Configs;
using ZhonTai.Plate.Admin.Domain.Dual;
using ZhonTai.Plate.Admin.Domain;

namespace ZhonTai.Plate.Admin.Repository
{
    public class CustomSyncData : SyncData, ISyncData
    {
        public virtual async Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null)
        {
            using (var uow = db.CreateUnitOfWork())
            using (var tran = uow.GetOrBeginTransaction())
            {
                var dualRepo = db.GetRepositoryBase<DualEntity>();
                dualRepo.UnitOfWork = uow;
                if (!await dualRepo.Select.AnyAsync())
                {
                    await dualRepo.InsertAsync(new DualEntity { });
                }

                var isTenant = appConfig.Tenant;

                var dictionaryTypes = GetData<DictionaryTypeEntity>(isTenant);
                await InitDataAsync(db, uow, tran, dictionaryTypes, dbConfig);

                var dictionaries = GetData<DictionaryEntity>(isTenant);
                await InitDataAsync(db, uow, tran, dictionaries, dbConfig);

                var users = GetData<UserEntity>(isTenant);
                await InitDataAsync(db, uow, tran, users, dbConfig);

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
}
