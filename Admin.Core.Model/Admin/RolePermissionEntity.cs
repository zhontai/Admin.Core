using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 角色权限
    /// </summary>
	[Table(Name = "ad_role_permission")]
    [Index("uk_role_permissioin_roleid_permissionid", "RoleId,PermissionId", true)]
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

        #region 外键 => 导航属性，ManyToMany
        /// <summary>
        /// 角色
        /// </summary>
        [Navigate("RoleId")]
        public RoleEntity Role { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        [Navigate("PermissionId")]
        public PermissionEntity Permission { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Column(IsIgnore = true)]
        public string RoleName { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        [Column(IsIgnore = true)]
        public string ApiPath { get; set; }
        #endregion
    }

}
