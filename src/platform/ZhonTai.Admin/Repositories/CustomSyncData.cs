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
            var sourceDataList = GetData<DictTypeEntity>(path: dbConfig.SyncDataPath);

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var sourceDataIds = sourceDataList.Select(e => e.Id).ToList();
            var dataList = await rep.Where(a => sourceDataIds.Contains(a.Id)).ToListAsync();

            //新增
            var dataIds = dataList.Select(a => a.Id).ToList();
            var insertDataList = sourceDataList.Where(a => !dataIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (dbConfig.SysUpdateData)
            {
                var updateDataList = dataList.Where(a => sourceDataIds.Contains(a.Id));
                if (updateDataList.Any())
                {
                    foreach (var data in updateDataList)
                    {
                        var sourceData = sourceDataList.Where(a => a.Id == data.Id).First();
                        sourceData.Adapt(data);
                    }

                    await rep.UpdateAsync(updateDataList);
                }
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
            var sourceDataList = GetData<DictEntity>(path: dbConfig.SyncDataPath);

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var sourceDataIds = sourceDataList.Select(e => e.Id).ToList();
            var dataList = await rep.Where(a => sourceDataIds.Contains(a.Id)).ToListAsync();

            //新增
            var dataIds = dataList.Select(a => a.Id).ToList();
            var insertDataList = sourceDataList.Where(a => !dataIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (dbConfig.SysUpdateData)
            {
                var updateDataList = dataList.Where(a => sourceDataIds.Contains(a.Id));
                if (updateDataList.Any())
                {
                    foreach (var data in updateDataList)
                    {
                        var sourceData = sourceDataList.Where(a => a.Id == data.Id).First();
                        sourceData.Adapt(data);
                    }

                    await rep.UpdateAsync(updateDataList);
                }
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
            var sourceDataList = GetData<UserEntity>(isTenant, dbConfig.SyncDataPath);

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var sourceDataIds = sourceDataList.Select(e => e.Id).ToList();
            var dataList = await rep.Where(a => sourceDataIds.Contains(a.Id)).ToListAsync();

            //新增
            var dataIds = dataList.Select(a => a.Id).ToList();
            var insertDataList = sourceDataList.Where(a => !dataIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (dbConfig.SysUpdateData)
            {
                var updateDataList = dataList.Where(a => sourceDataIds.Contains(a.Id));
                if (updateDataList.Any())
                {
                    foreach (var data in updateDataList)
                    {
                        var sourceData = sourceDataList.Where(a => a.Id == data.Id).First();
                        sourceData.Adapt(data);
                    }

                    await rep.UpdateAsync(updateDataList);
                }
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
            var sourceDataList = GetData<UserStaffEntity>(isTenant, dbConfig.SyncDataPath);

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var sourceDataIds = sourceDataList.Select(e => e.Id).ToList();
            var dataList = await rep.Where(a => sourceDataIds.Contains(a.Id)).ToListAsync();

            //新增
            var dataIds = dataList.Select(a => a.Id).ToList();
            var insertDataList = sourceDataList.Where(a => !dataIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (dbConfig.SysUpdateData)
            {
                var updateDataList = dataList.Where(a => sourceDataIds.Contains(a.Id));
                if (updateDataList.Any())
                {
                    foreach (var data in updateDataList)
                    {
                        var sourceData = sourceDataList.Where(a => a.Id == data.Id).First();
                        sourceData.Adapt(data);
                    }

                    await rep.UpdateAsync(updateDataList);
                }
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
            var sourceDataList = dataTree.ToList().ToPlainList((a) => a.Childs).ToArray();

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var sourceDataIds = sourceDataList.Select(e => e.Id).ToList();
            var dataList = await rep.Where(a => sourceDataIds.Contains(a.Id)).ToListAsync();

            //新增
            var dataIds = dataList.Select(a => a.Id).ToList();
            var insertDataList = sourceDataList.Where(a => !dataIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (dbConfig.SysUpdateData)
            {
                var updateDataList = dataList.Where(a => sourceDataIds.Contains(a.Id));
                if (updateDataList.Any())
                {
                    foreach (var data in updateDataList)
                    {
                        var sourceData = sourceDataList.Where(a => a.Id == data.Id).First();
                        sourceData.Adapt(data);
                    }

                    await rep.UpdateAsync(updateDataList);
                }
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
            var sourceDataList = GetData<RoleEntity>(isTenant, dbConfig.SyncDataPath);

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var sourceDataIds = sourceDataList.Select(e => e.Id).ToList();
            var dataList = await rep.Where(a => sourceDataIds.Contains(a.Id)).ToListAsync();

            //新增
            var dataIds = dataList.Select(a => a.Id).ToList();
            var insertDataList = sourceDataList.Where(a => !dataIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (dbConfig.SysUpdateData)
            {
                var updateDataList = dataList.Where(a => sourceDataIds.Contains(a.Id));
                if (updateDataList.Any())
                {
                    foreach (var data in updateDataList)
                    {
                        var sourceData = sourceDataList.Where(a => a.Id == data.Id).First();
                        sourceData.Adapt(data);
                    }

                    await rep.UpdateAsync(updateDataList);
                }
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
            var sourceDataList = dataTree.ToList().ToPlainList((a) => a.Childs).ToArray();

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var sourceDataIds = sourceDataList.Select(e => e.Id).ToList();
            var dataList = await rep.Where(a => sourceDataIds.Contains(a.Id)).ToListAsync();

            //新增
            var dataIds = dataList.Select(a => a.Id).ToList();
            var insertDataList = sourceDataList.Where(a => !dataIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (dbConfig.SysUpdateData)
            {
                var updateDataList = dataList.Where(a => sourceDataIds.Contains(a.Id));
                if (updateDataList.Any())
                {
                    foreach (var data in updateDataList)
                    {
                        var sourceData = sourceDataList.Where(a => a.Id == data.Id).First();
                        sourceData.Adapt(data);
                    }

                    await rep.UpdateAsync(updateDataList);
                }
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
            var sourceDataList = dataTree.ToList().ToPlainList((a) => a.Childs).ToArray();

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var sourceDataIds = sourceDataList.Select(e => e.Id).ToList();
            var dataList = await rep.Where(a => sourceDataIds.Contains(a.Id)).ToListAsync();

            //新增
            var dataIds = dataList.Select(a => a.Id).ToList();
            var insertDataList = sourceDataList.Where(a => !dataIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (dbConfig.SysUpdateData)
            {
                var updateDataList = dataList.Where(a => sourceDataIds.Contains(a.Id));
                if (updateDataList.Any())
                {
                    foreach (var data in updateDataList)
                    {
                        var sourceData = sourceDataList.Where(a => a.Id == data.Id).First();
                        sourceData.Adapt(data);
                    }

                    await rep.UpdateAsync(updateDataList);
                }
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
            var sourceDataList = dataTree.ToList().ToPlainList((a) => a.Childs).ToArray();

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var sourceDataIds = sourceDataList.Select(e => e.Id).ToList();
            var dataList = await rep.Where(a => sourceDataIds.Contains(a.Id)).ToListAsync();

            //新增
            var dataIds = dataList.Select(a => a.Id).ToList();
            var insertDataList = sourceDataList.Where(a => !dataIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (dbConfig.SysUpdateData)
            {
                var updateDataList = dataList.Where(a => sourceDataIds.Contains(a.Id));
                if (updateDataList.Any())
                {
                    foreach (var data in updateDataList)
                    {
                        var sourceData = sourceDataList.Where(a => a.Id == data.Id).First();
                        sourceData.Adapt(data);
                    }

                    await rep.UpdateAsync(updateDataList);
                }
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
            var dataList = await rep.Where(a => rep.Select.WithMemory(sourceDataList).Where(b => b.PermissionId == a.PermissionId && b.ApiId == a.ApiId).Any()).ToListAsync();

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
            var sourceDataList = GetData<TenantEntity>(path: dbConfig.SyncDataPath);

            if (!(sourceDataList?.Length > 0))
            {
                Console.WriteLine($"table: {tableName} import data []");
                return;
            }

            //查询
            var sourceDataIds = sourceDataList.Select(e => e.Id).ToList();
            var dataList = await rep.Where(a => sourceDataIds.Contains(a.Id)).ToListAsync();

            //新增
            var dataIds = dataList.Select(a => a.Id).ToList();
            var insertDataList = sourceDataList.Where(a => !dataIds.Contains(a.Id));
            if (insertDataList.Any())
            {
                await rep.InsertAsync(insertDataList);
            }

            //修改
            if (dbConfig.SysUpdateData)
            {
                var updateDataList = dataList.Where(a => sourceDataIds.Contains(a.Id));
                if (updateDataList.Any())
                {
                    foreach (var data in updateDataList)
                    {
                        var sourceData = sourceDataList.Where(a => a.Id == data.Id).First();
                        sourceData.Adapt(data);
                    }

                    await rep.UpdateAsync(updateDataList);
                }
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
