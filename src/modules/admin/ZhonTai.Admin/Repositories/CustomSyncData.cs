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
using ZhonTai.Common.Extensions;
using FreeSql;
using Mapster;
using ZhonTai.Admin.Domain.Region;

namespace ZhonTai.Admin.Repositories;

/// <summary>
/// 同步数据
/// </summary>
public class CustomSyncData : SyncData, ISyncData
{
    /// <summary>
    /// 用户角色记录
    /// </summary>
    /// <param name="UserId"></param>
    /// <param name="RoleId"></param>
    record UserRoleRecord(long UserId, long RoleId);

    /// <summary>
    /// 初始化用户角色
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    private async Task InitUserRoleAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        var tableName = GetTableName<UserRoleEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<UserRoleEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var sourceDataList = GetData<UserRoleEntity>(path: dbConfig.SyncDataPath);

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var userRoleRecordList = sourceDataList.Adapt<List<UserRoleRecord>>();
            var dataList = await rep.Where(a => rep.Orm.Select<UserRoleRecord>().WithMemory(userRoleRecordList).Where(b => b.UserId == a.UserId && b.RoleId == a.RoleId).Any()).ToListAsync();

            //新增
            var insertDataList = sourceDataList.Where(a => !(dataList.Where(b => a.UserId == b.UserId && a.RoleId == b.RoleId).Any())).ToList();
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            var msg = $"table: {tableName} sync data failed.\n{ex.Message}";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }
    }

    /// <summary>
    /// 用户部门记录
    /// </summary>
    /// <param name="UserId"></param>
    /// <param name="OrgId"></param>
    record UserOrgRecord(long UserId, long OrgId);

    /// <summary>
    /// 初始化用户部门
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    private async Task InitUserOrgAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        var tableName = GetTableName<UserOrgEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<UserOrgEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var sourceDataList = GetData<UserOrgEntity>(path: dbConfig.SyncDataPath);

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var userOrgRecordList = sourceDataList.Adapt<List<UserOrgRecord>>();
            var dataList = await rep.Where(a => rep.Orm.Select<UserOrgRecord>().WithMemory(userOrgRecordList).Where(b => b.UserId == a.UserId && b.OrgId == a.OrgId).Any()).ToListAsync();

            //新增
            var insertDataList = sourceDataList.Where(a => !(dataList.Where(b => a.UserId == b.UserId && a.OrgId == b.OrgId).Any())).ToList();
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            var msg = $"table: {tableName} sync data failed.\n{ex.Message}";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }
    }

    /// <summary>
    /// 角色权限记录
    /// </summary>
    /// <param name="RoleId"></param>
    /// <param name="PermissionId"></param>
    record RolePermissionRecord(long RoleId, long PermissionId);

    /// <summary>
    /// 初始化角色权限
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    private async Task InitRolePermissionAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        var tableName = GetTableName<RolePermissionEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<RolePermissionEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var sourceDataList = GetData<RolePermissionEntity>(path: dbConfig.SyncDataPath);

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var rolePermissionRecordList = sourceDataList.Adapt<List<RolePermissionRecord>>();
            var dataList = await rep.Where(a => rep.Orm.Select<RolePermissionRecord>().WithMemory(rolePermissionRecordList).Where(b => b.RoleId == a.RoleId && b.PermissionId == a.PermissionId).Any()).ToListAsync();

            //新增
            var insertDataList = sourceDataList.Where(a => !(dataList.Where(b => a.RoleId == b.RoleId && a.PermissionId == b.PermissionId).Any())).ToList();
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            var msg = $"table: {tableName} sync data failed.\n{ex.Message}";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }
    }

    /// <summary>
    /// 租户权限记录
    /// </summary>
    /// <param name="TenantId"></param>
    /// <param name="PermissionId"></param>
    record TenantPermissionRecord(long TenantId, long PermissionId);

    /// <summary>
    /// 初始化租户权限
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    private async Task InitTenantPermissionAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        var tableName = GetTableName<TenantPermissionEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<TenantPermissionEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var sourceDataList = GetData<TenantPermissionEntity>(path: dbConfig.SyncDataPath);

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var tenantPermissionRecordList = sourceDataList.Adapt<List<TenantPermissionRecord>>();
            var dataList = await rep.Where(a => rep.Orm.Select<TenantPermissionRecord>().WithMemory(tenantPermissionRecordList).Where(b => b.TenantId == a.TenantId && b.PermissionId == a.PermissionId).Any()).ToListAsync();

            //新增
            var insertDataList = sourceDataList.Where(a => !(dataList.Where(b => a.TenantId == b.TenantId && a.PermissionId == b.PermissionId).Any())).ToList();
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            var msg = $"table: {tableName} sync data failed.\n{ex.Message}";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }
    }

    /// <summary>
    /// 权限接口记录
    /// </summary>
    /// <param name="PermissionId"></param>
    /// <param name="ApiId"></param>
    record PermissionApiRecord(long PermissionId, long ApiId);

    /// <summary>
    /// 初始化权限接口
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    private async Task InitPermissionApiAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        var tableName = GetTableName<PermissionApiEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<PermissionApiEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var sourceDataList = GetData<PermissionApiEntity>(path: dbConfig.SyncDataPath);

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var dataRecordList = sourceDataList.Adapt<List<PermissionApiRecord>>();
            var dataList = await rep.Where(a => rep.Orm.Select<PermissionApiRecord>().WithMemory(dataRecordList).Where(b => b.PermissionId == a.PermissionId && b.ApiId == a.ApiId).Any()).ToListAsync();

            //新增
            var insertDataList = sourceDataList.Where(a => !(dataList.Where(b => a.PermissionId == b.PermissionId && a.ApiId == b.ApiId).Any())).ToList();
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            var msg = $"table: {tableName} sync data failed.\n{ex.Message}";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }
    }

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

            await SyncEntityAsync<RegionEntity>(db, unitOfWork, dbConfig, appConfig, processChilds: true);
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
            await SyncEntityAsync<TenantEntity>(db, unitOfWork, dbConfig, appConfig);
            await InitUserRoleAsync(db, unitOfWork, dbConfig);
            await InitUserOrgAsync(db, unitOfWork, dbConfig);
            await InitPermissionApiAsync(db, unitOfWork, dbConfig);
            await InitRolePermissionAsync(db, unitOfWork, dbConfig);
            await InitTenantPermissionAsync(db, unitOfWork, dbConfig);

            unitOfWork.Commit();
        }
        catch (Exception)
        {
            unitOfWork.Rollback();
            throw;
        }
    }
}
