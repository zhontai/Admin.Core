using System;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Service.Permission.Dto;

namespace ZhonTai.Plate.Admin.Service.Permission
{
    public partial interface IPermissionService
    {
        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetGroupAsync(long id);

        Task<IResultOutput> GetMenuAsync(long id);

        Task<IResultOutput> GetApiAsync(long id);

        Task<IResultOutput> GetDotAsync(long id);

        Task<IResultOutput> GetPermissionList();

        Task<IResultOutput> GetRolePermissionList(long roleId);


        Task<IResultOutput> GetTenantPermissionList(long tenantId);

        Task<IResultOutput> GetListAsync(string key, DateTime? start, DateTime? end);

        Task<IResultOutput> AddGroupAsync(PermissionAddGroupInput input);

        Task<IResultOutput> AddMenuAsync(PermissionAddMenuInput input);

        Task<IResultOutput> AddApiAsync(PermissionAddApiInput input);

        Task<IResultOutput> AddDotAsync(PermissionAddDotInput input);

        Task<IResultOutput> UpdateGroupAsync(PermissionUpdateGroupInput input);

        Task<IResultOutput> UpdateMenuAsync(PermissionUpdateMenuInput input);

        Task<IResultOutput> UpdateApiAsync(PermissionUpdateApiInput input);

        Task<IResultOutput> UpdateDotAsync(PermissionUpdateDotInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> SoftDeleteAsync(long id);

        Task<IResultOutput> AssignAsync(PermissionAssignInput input);

        Task<IResultOutput> SaveTenantPermissionsAsync(PermissionSaveTenantPermissionsInput input);
    }
}