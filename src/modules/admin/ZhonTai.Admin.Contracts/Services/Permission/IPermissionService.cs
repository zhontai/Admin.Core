using ZhonTai.Admin.Services.Permission.Dto;

namespace ZhonTai.Admin.Services.Permission;

/// <summary>
/// 权限接口
/// </summary>
public partial interface IPermissionService
{
    Task<PermissionGetGroupOutput> GetGroupAsync(long id);

    Task<PermissionGetMenuOutput> GetMenuAsync(long id);

    Task<PermissionGetDotOutput> GetDotAsync(long id);

    Task<IEnumerable<dynamic>> GetPermissionListAsync(string platform);

    Task<List<long>> GetRolePermissionListAsync(long roleId);

    Task<List<long>> GetTenantPermissionListAsync(long tenantId);

    Task<List<PermissionGetListOutput>> GetListAsync(PermissionGetListInput input);

    Task<long> AddGroupAsync(PermissionAddGroupInput input);

    Task<long> AddMenuAsync(PermissionAddMenuInput input);

    Task<long> AddDotAsync(PermissionAddDotInput input);

    Task UpdateGroupAsync(PermissionUpdateGroupInput input);

    Task UpdateMenuAsync(PermissionUpdateMenuInput input);

    Task UpdateDotAsync(PermissionUpdateDotInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);

    Task AssignAsync(PermissionAssignInput input);

    Task SaveTenantPermissionsAsync(PermissionSaveTenantPermissionsInput input);
}