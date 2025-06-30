using ZhonTai.Admin.Domain.DictType;
using ZhonTai.Admin.Domain.Dict;
using ZhonTai.Admin.Domain.Api;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Domain.Tenant;
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
using ZhonTai.Admin.Domain.PrintTemplate;

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
    /// 打印模板记录
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Code"></param>
    record PrintTemplateRecord(long Id, string Name, string Code);

    /// <summary>
    /// 地区记录
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Code"></param>
    record RegionRecord(long Id, string Name, string Code);

    /// <summary>
    /// 数据字典分类记录
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    record DictTypeRecord(long Id, string Name);

    /// <summary>
    /// 数据字典记录
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="DictTypeId"></param>
    /// <param name="Name"></param>
    record DictRecord(long Id, long DictTypeId, string Name);

    /// <summary>
    /// 用户记录
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="UserName"></param>
    record UserRecord(long Id, string UserName);

    /// <summary>
    /// 部门记录
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="TenantId"></param>
    /// <param name="ParentId"></param>
    /// <param name="Name"></param>
    record OrgRecord(long Id, long TenantId, long ParentId, string Name);

    /// <summary>
    /// 角色记录
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="TenantId"></param>
    /// <param name="ParentId"></param>
    /// <param name="Name"></param>
    record RoleRecord(long Id, long TenantId, long ParentId, string Name);

    /// <summary>
    /// 接口记录
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="ParentId"></param>
    /// <param name="Path"></param>
    record ApiRecord(long Id, long ParentId, string Path);

    /// <summary>
    /// 视图记录
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Platform"></param>
    /// <param name="ParentId"></param>
    /// <param name="Label"></param>
    record ViewRecord(long Id, string Platform, long ParentId, string Label);

    /// <summary>
    /// 权限记录
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Platform"></param>
    /// <param name="ParentId"></param>
    /// <param name="Label"></param>
    record PermissionRecord(long Id, string Platform, long ParentId, string Label);

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

            await SyncEntityAsync<PrintTemplateEntity>(db, unitOfWork, dbConfig, appConfig, 
            whereFunc: (select, batchDataList) =>
            {
                var recordList = batchDataList.Adapt<List<PrintTemplateRecord>>();
                return select.Where(a =>
                    db.Select<PrintTemplateRecord>().WithMemory(recordList)
                    .Any(b => a.Id == b.Id 
                    || (!string.IsNullOrWhiteSpace(a.Name) && a.Name == b.Name)
                    || (!string.IsNullOrWhiteSpace(a.Code) && a.Code == b.Code))
                );
            },
            insertDataFunc: (batchDataList, dbDataList) =>
            {
                return batchDataList.Where(a => !dbDataList.Any(b => a.Id == b.Id
                    || (!string.IsNullOrWhiteSpace(a.Name) && a.Name == b.Name)
                    || (!string.IsNullOrWhiteSpace(a.Code) && a.Code == b.Code)));
            });
            await SyncEntityAsync<RegionEntity>(db, unitOfWork, dbConfig, appConfig, processChilds: true, 
            whereFunc: (select, batchDataList) =>
            {
                var recordList = batchDataList.Adapt<List<RegionRecord>>();
                return select.Where(a =>
                    db.Select<RegionRecord>().WithMemory(recordList)
                    .Any(b => a.Id == b.Id
                    || (!string.IsNullOrWhiteSpace(a.Name) && a.Name == b.Name)
                    || (!string.IsNullOrWhiteSpace(a.Code) && a.Code == b.Code))
                );
            },
            insertDataFunc: (batchDataList, dbDataList) =>
            {
                return batchDataList.Where(a => !dbDataList.Any(b => a.Id == b.Id
                    || (!string.IsNullOrWhiteSpace(a.Name) && a.Name == b.Name)
                    || (!string.IsNullOrWhiteSpace(a.Code) && a.Code == b.Code)));
            });
            await SyncEntityAsync<DictTypeEntity>(db, unitOfWork, dbConfig, appConfig, 
            whereFunc: (select, batchDataList) =>
            {
                var recordList = batchDataList.Adapt<List<DictTypeRecord>>();
                return select.Where(a =>
                    db.Select<DictTypeRecord>().WithMemory(recordList)
                    .Any(b => a.Id == b.Id || (!string.IsNullOrWhiteSpace(a.Name) && a.Name == b.Name))
                );
            },
            insertDataFunc: (batchDataList, dbDataList) =>
            {
                return batchDataList.Where(a => !dbDataList.Any(b => a.Id == b.Id || (!string.IsNullOrWhiteSpace(a.Name) && a.Name == b.Name)));
            });
            await SyncEntityAsync<DictEntity>(db, unitOfWork, dbConfig, appConfig, 
            whereFunc: (select, batchDataList) =>
            {
                var recordList = batchDataList.Adapt<List<DictRecord>>();
                return select.Where(a =>
                    db.Select<DictRecord>().WithMemory(recordList)
                    .Any(b => a.Id == b.Id || (a.DictTypeId == b.DictTypeId && !string.IsNullOrWhiteSpace(a.Name) && a.Name == b.Name))
                );
            },
            insertDataFunc: (batchDataList, dbDataList) =>
            {
                return batchDataList.Where(a => !dbDataList.Any(b => a.Id == b.Id || (a.DictTypeId == b.DictTypeId && !string.IsNullOrWhiteSpace(a.Name) && a.Name == b.Name)));
            });
            await SyncEntityAsync<UserEntity>(db, unitOfWork, dbConfig, appConfig, 
            whereFunc: (select, batchDataList) =>
            {
                var recordList = batchDataList.Adapt<List<UserRecord>>();
                return select.Where(a =>
                    db.Select<UserRecord>().WithMemory(recordList)
                    .Any(b => a.Id == b.Id || (!string.IsNullOrWhiteSpace(a.UserName) && a.UserName == b.UserName))
                );
            });
            await SyncEntityAsync<UserStaffEntity>(db, unitOfWork, dbConfig, appConfig);
            await SyncEntityAsync<OrgEntity>(db, unitOfWork, dbConfig, appConfig, processChilds: true, 
            whereFunc: (select, batchDataList) =>
            {
                var recordList = batchDataList.Adapt<List<OrgRecord>>();
                return select.Where(a =>
                    db.Select<OrgRecord>().WithMemory(recordList)
                    .Any(b => a.Id == b.Id || (a.TenantId == b.TenantId && a.ParentId == b.ParentId && !string.IsNullOrWhiteSpace(a.Name) && a.Name == b.Name))
                );
            },
            insertDataFunc: (batchDataList, dbDataList) =>
            {
                return batchDataList.Where(a => !dbDataList.Any(b => a.Id == b.Id));
            });
            await SyncEntityAsync<RoleEntity>(db, unitOfWork, dbConfig, appConfig, 
            whereFunc: (select, batchDataList) =>
            {
                var recordList = batchDataList.Adapt<List<RoleRecord>>();
                return select.Where(a =>
                    db.Select<RoleRecord>().WithMemory(recordList)
                    .Any(b => a.Id == b.Id || (a.TenantId == b.TenantId && a.ParentId == b.ParentId && !string.IsNullOrWhiteSpace(a.Name) && a.Name == b.Name))
                );
            },
            insertDataFunc: (batchDataList, dbDataList) =>
            {
                return batchDataList.Where(a => !dbDataList.Any(b => a.Id == b.Id || (a.TenantId == b.TenantId && a.ParentId == b.ParentId && !string.IsNullOrWhiteSpace(a.Name) && a.Name == b.Name)));
            });
            await SyncEntityAsync<ApiEntity>(db, unitOfWork, dbConfig, appConfig, processChilds: true, 
            whereFunc: (select, batchDataList) =>
            {
                var recordList = batchDataList.Adapt<List<ApiRecord>>();
                return select.Where(a =>
                    db.Select<ApiRecord>().WithMemory(recordList)
                    .Any(b => a.Id == b.Id || (a.ParentId == b.ParentId && !string.IsNullOrWhiteSpace(a.Path) && a.Path == b.Path))
                );
            },
            insertDataFunc: (batchDataList, dbDataList) =>
            {
                return batchDataList.Where(a => !dbDataList.Any(b => a.Id == b.Id || (a.ParentId == b.ParentId && !string.IsNullOrWhiteSpace(a.Path) && a.Path == b.Path)));
            });
            await SyncEntityAsync<ViewEntity>(db, unitOfWork, dbConfig, appConfig, processChilds: true, 
            whereFunc: (select, batchDataList) =>
            {
                var recordList = batchDataList.Adapt<List<ViewRecord>>();
                return select.Where(a =>
                    db.Select<ViewRecord>().WithMemory(recordList)
                    .Any(b => a.Id == b.Id 
                    || (!string.IsNullOrWhiteSpace(a.Platform) && a.Platform == b.Platform
                    && a.ParentId == b.ParentId && !string.IsNullOrWhiteSpace(a.Label) && a.Label == b.Label))
                );
            },
            insertDataFunc: (batchDataList, dbDataList) =>
            {
                return batchDataList.Where(a => !dbDataList.Any(b => a.Id == b.Id 
                    || (!string.IsNullOrWhiteSpace(a.Platform) && a.Platform == b.Platform
                    && a.ParentId == b.ParentId && !string.IsNullOrWhiteSpace(a.Label) && a.Label == b.Label)));
            });
            await SyncEntityAsync<PermissionEntity>(db, unitOfWork, dbConfig, appConfig, processChilds: true, 
            whereFunc: (select, batchDataList) =>
            {
                var recordList = batchDataList.Adapt<List<PermissionRecord>>();
                return select.Where(a =>
                    db.Select<PermissionRecord>().WithMemory(recordList)
                    .Any(b => a.Id == b.Id 
                    || (!string.IsNullOrWhiteSpace(a.Platform) && a.Platform == b.Platform
                    && a.ParentId == b.ParentId && !string.IsNullOrWhiteSpace(a.Label) && a.Label == b.Label))
                );
            },
            insertDataFunc: (batchDataList, dbDataList) =>
            {
                return batchDataList.Where(a => !dbDataList.Any(b => a.Id == b.Id
                    || (!string.IsNullOrWhiteSpace(a.Platform) && a.Platform == b.Platform
                    && a.ParentId == b.ParentId && !string.IsNullOrWhiteSpace(a.Label) && a.Label == b.Label)));
            });
            await SyncEntityAsync<TenantEntity>(db, unitOfWork, dbConfig, appConfig);
            await InitUserRoleAsync(db, unitOfWork, dbConfig);
            await InitUserOrgAsync(db, unitOfWork, dbConfig);
            await InitPermissionApiAsync(db, unitOfWork, dbConfig);
            await InitRolePermissionAsync(db, unitOfWork, dbConfig);

            unitOfWork.Commit();
        }
        catch (Exception)
        {
            unitOfWork.Rollback();
            throw;
        }
    }
}
