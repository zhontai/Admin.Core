using Admin.Core.Model.Admin;

namespace Admin.Core.Db
{
    /// <summary>
    /// 数据
    /// </summary>
    public class Data
    {
        public DictionaryEntity[] Dictionaries { get; set; }
        public ApiEntity[] Apis { get; set; }
        public ViewEntity[] Views { get; set; }
        public PermissionEntity[] Permissions { get; set; }
        public UserEntity[] Users { get; set; }
        public RoleEntity[] Roles { get; set; }
        public UserRoleEntity[] UserRoles { get; set; }
        public RolePermissionEntity[] RolePermissions { get; set; }
        public TenantEntity[] Tenants { get; set; }
    }
}
