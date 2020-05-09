using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 用户角色
    /// </summary>
	[Table(Name = "ad_user_role")]
    [Index("uk_user_role_userid_roleid", nameof(UserId) + "," + nameof(RoleId), true)]
    public class UserRoleEntity: EntityAdd
    {
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
