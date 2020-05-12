using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 角色权限
    /// </summary>
	[Table(Name = "ad_role_permission")]
    [Index("uk_role_permissioin_roleid_permissionid", nameof(RoleId) + "," + nameof(PermissionId), true)]
    public class RolePermissionEntity: EntityAdd
    {
        /// <summary>
        /// 角色Id
        /// </summary>
		public long RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
		public long PermissionId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public RoleEntity Role { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public PermissionEntity Permission { get; set; }
    }

}
