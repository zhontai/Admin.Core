using System.Collections.Generic;

namespace Admin.Core.Service.Admin.Permission.Input
{
    public class PermissionAssignInput
    {
        public List<int> PermissionIds { get; set; }
        public int RoleId { get; set; }
    }
}
