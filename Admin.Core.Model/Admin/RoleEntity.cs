using System;
using FreeSql.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 角色
    /// </summary>
	[Table(Name = "ad_role")]
    [Index("uk_role_name", "Name", true)]
    public class RoleEntity: EntityBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Column(DbType = "varchar(100)")]
		public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;

        /// <summary>
        /// 排序
        /// </summary>
		public int Sort { get; set; }

        [Navigate(ManyToMany = typeof(UserRoleEntity))]
        public virtual ICollection<UserEntity> Users { get; set; }
    }

}
