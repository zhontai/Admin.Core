using Admin.Core.Model.Admin;

namespace Admin.Core.Repository
{
    /// <summary>
    /// 数据
    /// </summary>
    public class Data
    {
        //public DictionaryEntity[] Dictionaries { get; set; }
        public ApiEntity[] Apis { get; set; }

        public ApiEntity[] ApiTree { get; set; }
        public ViewEntity[] ViewTree { get; set; }
        public PermissionEntity[] PermissionTree { get; set; }
        public UserEntity[] Users { get; set; }
        public RoleEntity[] Roles { get; set; }
        public UserRoleEntity[] UserRoles { get; set; }
        public RolePermissionEntity[] RolePermissions { get; set; }
        public TenantEntity[] Tenants { get; set; }
    }
}