using Admin.Core.Model.Admin;
using Admin.Core.Model.Personnel;

namespace Admin.Core.Repository
{
    /// <summary>
    /// 数据
    /// </summary>
    public class Data
    {
        public DictionaryTypeEntity[] DictionaryTypes { get; set; }
        public DictionaryEntity[] Dictionaries { get; set; }
        public ApiEntity[] Apis { get; set; }
        public ApiEntity[] ApiTree { get; set; }
        public ViewEntity[] ViewTree { get; set; }
        public PermissionEntity[] PermissionTree { get; set; }
        public UserEntity[] Users { get; set; }
        public RoleEntity[] Roles { get; set; }
        public UserRoleEntity[] UserRoles { get; set; }
        public RolePermissionEntity[] RolePermissions { get; set; }
        public TenantEntity[] Tenants { get; set; }
        public TenantPermissionEntity[] TenantPermissions { get; set; }
        public PermissionApiEntity[] PermissionApis { get; set; }

        public PositionEntity[] Positions { get; set; }
        public OrganizationEntity[] OrganizationTree { get; set; }
        public EmployeeEntity[] Employees { get; set; }
    }
}