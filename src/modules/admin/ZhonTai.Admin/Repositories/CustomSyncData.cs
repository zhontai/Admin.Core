using System.Threading.Tasks;
using ZhonTai.Admin.Domain.DictType;
using ZhonTai.Admin.Domain.Dict;
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
using System;
using FreeSql;
using Mapster;
using System.Collections.Generic;

namespace ZhonTai.Admin.Repositories;

/// <summary>
/// 同步数据
/// </summary>
public class CustomSyncData : SyncData, ISyncData
{
    /// <summary>
    /// 同步数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="dbConfig"></param>
    /// <param name="appConfig"></param>
    /// <returns></returns>
    public virtual async Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null)
    {
        using var unitOfWork = db.CreateUnitOfWork();

        try
        {
            var isTenant = appConfig.Tenant;

            await SyncEntityAsync<DictTypeEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<DictEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<UserEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<UserStaffEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<DictTypeEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<OrgEntity>(db, unitOfWork, dbConfig, appConfig, processChilds: true);
            await SyncEntityAsync<RoleEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<ApiEntity>(db, unitOfWork, dbConfig, appConfig, processChilds: true);
            await SyncEntityAsync<ViewEntity>(db, unitOfWork, dbConfig, appConfig, processChilds: true);
            await SyncEntityAsync<PermissionEntity>(db, unitOfWork, dbConfig, appConfig, processChilds: true);
            await SyncEntityAsync<PermissionApiEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<UserRoleEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<UserOrgEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<RolePermissionEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<TenantEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<TenantPermissionEntity>(db, unitOfWork, dbConfig, appConfig);

            unitOfWork.Commit();
        }
        catch (Exception)
        {
            unitOfWork.Rollback();
            throw;
        }
    }
}
