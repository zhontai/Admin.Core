using System;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Service.Permission.Input;

namespace ZhonTai.Plate.Admin.Service.Permission
{
    public partial interface IPermissionService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> GetGroupAsync(long id);

        Task<IResponseOutput> GetMenuAsync(long id);

        Task<IResponseOutput> GetApiAsync(long id);

        Task<IResponseOutput> GetDotAsync(long id);

        Task<IResponseOutput> GetPermissionList();

        Task<IResponseOutput> GetRolePermissionList(long roleId);


        Task<IResponseOutput> GetTenantPermissionList(long tenantId);

        Task<IResponseOutput> GetListAsync(string key, DateTime? start, DateTime? end);

        Task<IResponseOutput> AddGroupAsync(PermissionAddGroupInput input);

        Task<IResponseOutput> AddMenuAsync(PermissionAddMenuInput input);

        Task<IResponseOutput> AddApiAsync(PermissionAddApiInput input);

        Task<IResponseOutput> AddDotAsync(PermissionAddDotInput input);

        Task<IResponseOutput> UpdateGroupAsync(PermissionUpdateGroupInput input);

        Task<IResponseOutput> UpdateMenuAsync(PermissionUpdateMenuInput input);

        Task<IResponseOutput> UpdateApiAsync(PermissionUpdateApiInput input);

        Task<IResponseOutput> UpdateDotAsync(PermissionUpdateDotInput input);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<IResponseOutput> SoftDeleteAsync(long id);

        Task<IResponseOutput> AssignAsync(PermissionAssignInput input);

        Task<IResponseOutput> SaveTenantPermissionsAsync(PermissionSaveTenantPermissionsInput input);
    }
}