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
using FreeSql.DataAnnotations;
using System;
using FreeSql;

namespace ZhonTai.Admin.Repositories;

public class CustomSyncData : SyncData, ISyncData
{
    /// <summary>
    /// 获得表名
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static string GetTableName<T>() where T : class, new()
    {
        var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
        return table.Name;
    }

    private static bool IsSyncData(string tableName, DbConfig dbConfig)
    {
        var isSyncData = true;

        var hasDataIncludeTables = dbConfig.SyncDataIncludeTables?.Length > 0;
        if (hasDataIncludeTables && !dbConfig.SyncDataIncludeTables.Contains(tableName))
        {
            isSyncData = false;
        }

        var hasSyncDataExcludeTables = dbConfig.SyncDataExcludeTables?.Length > 0;
        if (hasSyncDataExcludeTables && dbConfig.SyncDataExcludeTables.Contains(tableName))
        {
            isSyncData = false;
        }

        return isSyncData;
    }

    /// <summary>
    /// 初始化字典类型
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    private async Task InitDictTypeAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        var tableName = GetTableName<DictTypeEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<DictTypeEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var dataList = GetData<DictTypeEntity>(path: dbConfig.SyncDataPath);

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var ids = dataList.Select(e => e.Id).ToList();
            var recordList = await rep.Where(a => ids.Contains(a.Id)).ToListAsync();

            //新增
            var recordIds = recordList.Select(a => a.Id).ToList();
            var insertDataList = dataList.Where(a => !recordIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (recordList?.Count > 0)
            {
                var updateDataList = dataList.Where(a => recordIds.Contains(a.Id));
                await rep.UpdateAsync(updateDataList);
            }

            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// 初始化字典
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    private async Task InitDictAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        var tableName = GetTableName<DictEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<DictEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var dataList = GetData<DictEntity>(path: dbConfig.SyncDataPath);

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var ids = dataList.Select(e => e.Id).ToList();
            var recordList = await rep.Where(a => ids.Contains(a.Id)).ToListAsync();

            //新增
            var recordIds = recordList.Select(a => a.Id).ToList();
            var insertDataList = dataList.Where(a => !recordIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (recordList?.Count > 0)
            {
                var updateDataList = dataList.Where(a => recordIds.Contains(a.Id));
                await rep.UpdateAsync(updateDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// 初始化用户
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <param name="isTenant"></param>
    /// <returns></returns>
    private async Task InitUserAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig, bool isTenant)
    {
        var tableName = GetTableName<UserEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<UserEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var dataList = GetData<UserEntity>(isTenant, dbConfig.SyncDataPath);

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var ids = dataList.Select(e => e.Id).ToList();
            var recordList = await rep.Where(a => ids.Contains(a.Id)).ToListAsync();

            //新增
            var recordIds = recordList.Select(a => a.Id).ToList();
            var insertDataList = dataList.Where(a => !recordIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (recordList?.Count > 0)
            {
                var updateDataList = dataList.Where(a => recordIds.Contains(a.Id));
                await rep.UpdateAsync(updateDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// 初始化用户员工
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <param name="isTenant"></param>
    /// <returns></returns>
    private async Task InitUserStaffAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig, bool isTenant)
    {
        var tableName = GetTableName<UserStaffEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<UserStaffEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var dataList = GetData<UserStaffEntity>(isTenant, dbConfig.SyncDataPath);

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var ids = dataList.Select(e => e.Id).ToList();
            var recordList = await rep.Where(a => ids.Contains(a.Id)).ToListAsync();

            //新增
            var recordIds = recordList.Select(a => a.Id).ToList();
            var insertDataList = dataList.Where(a => !recordIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (recordList?.Count > 0)
            {
                var updateDataList = dataList.Where(a => recordIds.Contains(a.Id));
                await rep.UpdateAsync(updateDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// 初始化部门
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <param name="isTenant"></param>
    /// <returns></returns>
    private async Task InitOrgAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig, bool isTenant)
    {
        var tableName = GetTableName<OrgEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<OrgEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var dataTree = GetData<OrgEntity>(isTenant, dbConfig.SyncDataPath);
            var dataList = dataTree.ToList().ToPlainList((a) => a.Childs).ToArray();

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var ids = dataList.Select(e => e.Id).ToList();
            var recordList = await rep.Where(a => ids.Contains(a.Id)).ToListAsync();

            //新增
            var recordIds = recordList.Select(a => a.Id).ToList();
            var insertDataList = dataList.Where(a => !recordIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (recordList?.Count > 0)
            {
                var updateDataList = dataList.Where(a => recordIds.Contains(a.Id));
                await rep.UpdateAsync(updateDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// 初始化角色
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <param name="isTenant"></param>
    /// <returns></returns>
    private async Task InitRoleAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig, bool isTenant)
    {
        var tableName = GetTableName<RoleEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<RoleEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var dataList = GetData<RoleEntity>(isTenant, dbConfig.SyncDataPath);

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var ids = dataList.Select(e => e.Id).ToList();
            var recordList = await rep.Where(a => ids.Contains(a.Id)).ToListAsync();

            //新增
            var recordIds = recordList.Select(a => a.Id).ToList();
            var insertDataList = dataList.Where(a => !recordIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (recordList?.Count > 0)
            {
                var updateDataList = dataList.Where(a => recordIds.Contains(a.Id));
                await rep.UpdateAsync(updateDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// 初始化接口
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    private async Task InitApiAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        var tableName = GetTableName<ApiEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<ApiEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var dataTree = GetData<ApiEntity>(path: dbConfig.SyncDataPath);
            var dataList = dataTree.ToList().ToPlainList((a) => a.Childs).ToArray();

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var ids = dataList.Select(e => e.Id).ToList();
            var recordList = await rep.Where(a => ids.Contains(a.Id)).ToListAsync();

            //新增
            var recordIds = recordList.Select(a => a.Id).ToList();
            var insertDataList = dataList.Where(a => !recordIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (recordList?.Count > 0)
            {
                var updateDataList = dataList.Where(a => recordIds.Contains(a.Id));
                await rep.UpdateAsync(updateDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// 初始化视图
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    private async Task InitViewAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        var tableName = GetTableName<ViewEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<ViewEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var dataTree = GetData<ViewEntity>(path: dbConfig.SyncDataPath);
            var dataList = dataTree.ToList().ToPlainList((a) => a.Childs).ToArray();

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var ids = dataList.Select(e => e.Id).ToList();
            var recordList = await rep.Where(a => ids.Contains(a.Id)).ToListAsync();

            //新增
            var recordIds = recordList.Select(a => a.Id).ToList();
            var insertDataList = dataList.Where(a => !recordIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (recordList?.Count > 0)
            {
                var updateDataList = dataList.Where(a => recordIds.Contains(a.Id));
                await rep.UpdateAsync(updateDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// 初始化权限
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    private async Task InitPermissionAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        var tableName = GetTableName<PermissionEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<PermissionEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var dataTree = GetData<PermissionEntity>(path: dbConfig.SyncDataPath);
            var dataList = dataTree.ToList().ToPlainList((a) => a.Childs).ToArray();

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var ids = dataList.Select(e => e.Id).ToList();
            var recordList = await rep.Where(a => ids.Contains(a.Id)).ToListAsync();

            //新增
            var recordIds = recordList.Select(a => a.Id).ToList();
            var insertDataList = dataList.Where(a => !recordIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (recordList?.Count > 0)
            {
                var updateDataList = dataList.Where(a => recordIds.Contains(a.Id));
                await rep.UpdateAsync(updateDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

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
            var dataList = GetData<PermissionApiEntity>(path: dbConfig.SyncDataPath);

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var permissionIds = dataList.Select(a => a.PermissionId).ToList();
            var apiIds = dataList.Select(a => a.ApiId).ToList();
            var records = await rep.Where(a => permissionIds.Contains(a.PermissionId) && apiIds.Contains(a.ApiId)).ToListAsync();

            //新增
            var recordPermissionIds = dataList.Select(a => a.PermissionId).ToList();
            var recordApiIds = dataList.Select(a => a.ApiId).ToList();
            var insertDataList = dataList.Where(a => !(recordPermissionIds.Contains(a.PermissionId) && recordApiIds.Contains(a.ApiId)));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

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
            var dataList = GetData<UserRoleEntity>(path: dbConfig.SyncDataPath);

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var userIds = dataList.Select(a => a.UserId).ToList();
            var roleIds = dataList.Select(a => a.RoleId).ToList();
            var records = await rep.Where(a => userIds.Contains(a.UserId) && roleIds.Contains(a.RoleId)).ToListAsync();

            //新增
            var recordUserIds = dataList.Select(a => a.UserId).ToList();
            var recordRoleIds = dataList.Select(a => a.RoleId).ToList();
            var insertDataList = dataList.Where(a => !(recordUserIds.Contains(a.UserId) && recordRoleIds.Contains(a.RoleId)));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

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
            var dataList = GetData<UserOrgEntity>(path: dbConfig.SyncDataPath);

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var userIds = dataList.Select(a => a.UserId).ToList();
            var orgIds = dataList.Select(a => a.OrgId).ToList();
            var records = await rep.Where(a => userIds.Contains(a.UserId) && orgIds.Contains(a.OrgId)).ToListAsync();

            //新增
            var recordUserIds = dataList.Select(a => a.UserId).ToList();
            var recordOrgIds = dataList.Select(a => a.OrgId).ToList();
            var insertDataList = dataList.Where(a => !(recordUserIds.Contains(a.UserId) && recordOrgIds.Contains(a.OrgId)));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

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
            var dataList = GetData<RolePermissionEntity>(path: dbConfig.SyncDataPath);

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var roleIds = dataList.Select(a => a.RoleId).ToList();
            var permissionIds = dataList.Select(a => a.PermissionId).ToList();
            var records = await rep.Where(a => roleIds.Contains(a.RoleId) && permissionIds.Contains(a.PermissionId)).ToListAsync();

            //新增
            var recordRoleIds = dataList.Select(a => a.RoleId).ToList();
            var recordPermissionIds = dataList.Select(a => a.PermissionId).ToList();
            var insertDataList = dataList.Where(a => !(recordRoleIds.Contains(a.RoleId) && recordPermissionIds.Contains(a.PermissionId)));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// 初始化租户
    /// </summary>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    private async Task InitTenantAsync(IFreeSql db, IRepositoryUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        var tableName = GetTableName<TenantEntity>();
        try
        {
            if (!IsSyncData(tableName, dbConfig))
            {
                return;
            }

            var rep = db.GetRepository<TenantEntity>();
            rep.UnitOfWork = unitOfWork;

            //数据列表
            var dataList = GetData<TenantEntity>(path: dbConfig.SyncDataPath);

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var ids = dataList.Select(e => e.Id).ToList();
            var recordList = await rep.Where(a => ids.Contains(a.Id)).ToListAsync();

            //新增
            var recordIds = recordList.Select(a => a.Id).ToList();
            var insertDataList = dataList.Where(a => !recordIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (recordList?.Count > 0)
            {
                var updateDataList = dataList.Where(a => recordIds.Contains(a.Id));
                await rep.UpdateAsync(updateDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }

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
            var dataList = GetData<TenantPermissionEntity>(path: dbConfig.SyncDataPath);

            if (!(dataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var tenantIds = dataList.Select(a => a.TenantId).ToList();
            var permissionIds = dataList.Select(a => a.PermissionId).ToList();
            var records = await rep.Where(a => tenantIds.Contains(a.TenantId) && permissionIds.Contains(a.PermissionId)).ToListAsync();

            //新增
            var recordTenantIds = dataList.Select(a => a.TenantId).ToList();
            var recordPermissionIds = dataList.Select(a => a.PermissionId).ToList();
            var insertDataList = dataList.Where(a => !(tenantIds.Contains(a.TenantId) && recordPermissionIds.Contains(a.PermissionId)));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }
            Console.WriteLine($"table: {tableName} sync data succeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"table: {tableName} sync data failed.\n{ex.Message}");
            throw;
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

            await InitDictTypeAsync(db, unitOfWork, dbConfig);

            await InitDictAsync(db, unitOfWork, dbConfig);

            await InitUserAsync(db, unitOfWork, dbConfig, isTenant);

            await InitUserStaffAsync(db, unitOfWork, dbConfig, isTenant);

            await InitOrgAsync(db, unitOfWork, dbConfig, isTenant);

            await InitRoleAsync(db, unitOfWork, dbConfig, isTenant);

            await InitApiAsync(db, unitOfWork, dbConfig);

            await InitViewAsync(db, unitOfWork, dbConfig);

            await InitPermissionAsync(db, unitOfWork, dbConfig);

            await InitPermissionApiAsync(db, unitOfWork, dbConfig);

            await InitUserRoleAsync(db, unitOfWork, dbConfig);

            await InitUserOrgAsync(db, unitOfWork, dbConfig);

            await InitRolePermissionAsync(db, unitOfWork, dbConfig);

            await InitTenantAsync(db, unitOfWork, dbConfig);

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
