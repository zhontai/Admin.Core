using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 用户角色
    /// </summary>
	[Table(Name = "ad_user_role")]
    [Index("idx_{tablename}_01", nameof(UserId) + "," + nameof(RoleId), true)]
    public class UserRoleEntity: EntityAdd, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10, CanUpdate = false)]
        public long? TenantId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        
        public UserEntity User { get; set; }
        
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }

		public RoleEntity Role { get; set; }
    }
}
