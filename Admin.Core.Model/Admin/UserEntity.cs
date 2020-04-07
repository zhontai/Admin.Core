using System;
using FreeSql.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 用户
    /// </summary>
	[Table(Name = "ad_user")]
    [Index("uk_user_username", nameof(UserName), true)]
    public class UserEntity: EntityBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        [MaxLength(60)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(60)]
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(60)]
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(60)]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [MaxLength(100)]
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
