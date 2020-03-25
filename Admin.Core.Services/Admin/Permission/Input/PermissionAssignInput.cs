using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.Core.Service.Admin.Permission.Input
{
    public class PermissionAssignInput
    {
        [Required(ErrorMessage = "角色不能为空！")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "权限不能为空！")]
        public List<int> PermissionIds { get; set; }
    }
}
