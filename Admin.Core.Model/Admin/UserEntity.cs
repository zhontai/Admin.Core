using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 用户
    /// </summary>
	[Table(Name = "ad_user")]
    [Index("idx_{tablename}_01", nameof(UserName) + "," + nameof(TenantId), true)]
    public class UserEntity : EntityFull, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10)]
        public long? TenantId { get; set; }

        public TenantEntity Tenant { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Column(StringLength = 60)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column(StringLength = 60)]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Column(StringLength = 60)]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Column(StringLength = 100)]
        public string Avatar { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column(StringLength = 500)]
        public string Remark { get; set; }

        [Navigate(ManyToMany = typeof(UserRoleEntity))]
        public ICollection<RoleEntity> Roles { get; set; }
    }
}