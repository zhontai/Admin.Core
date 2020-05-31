
using System;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Admin.Permission.Input;

namespace Admin.Core.Service.Admin.Permission
{
    public partial interface IPermissionService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> GetGroupAsync(long id);

        Task<IResponseOutput> GetMenuAsync(long id);

        Task<IResponseOutput> GetApiAsync(long id);

        Task<IResponseOutput> GetDotAsync(long id);

        Task<IResponseOutput> GetPermissionList();

        Task<IResponseOutput> GetRolePermissionList(long roleId = 0);

        Task<IResponseOutput> ListAsync(string key, DateTime? start, DateTime? end);

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
    }
}