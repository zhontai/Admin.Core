using ZhonTai.Plate.Admin.Domain.Api;
using ZhonTai.Plate.Admin.Domain.Dictionary;
using ZhonTai.Plate.Admin.Domain.DictionaryType;
using ZhonTai.Plate.Admin.Domain.Permission;
using ZhonTai.Plate.Admin.Domain.PermissionApi;
using ZhonTai.Plate.Admin.Domain.Role;
using ZhonTai.Plate.Admin.Domain.RolePermission;
using ZhonTai.Plate.Admin.Domain.Tenant;
using ZhonTai.Plate.Admin.Domain.TenantPermission;
using ZhonTai.Plate.Admin.Domain.User;
using ZhonTai.Plate.Admin.Domain.UserRole;
using ZhonTai.Plate.Admin.Domain.View;

namespace ZhonTai.Plate.Admin.Repository
{
    /// <summary>
    /// 数据
    /// </summary>
    public partial class Data
    {
        #region Admin
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
        #endregion
    }
}