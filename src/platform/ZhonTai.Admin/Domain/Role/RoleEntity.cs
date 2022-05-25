using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Domain.RolePermission;

namespace ZhonTai.Admin.Domain.Role
{
    /// <summary>
    /// 角色
    /// </summary>
	[Table(Name = "ad_role")]
    [Index("idx_{tablename}_01", nameof(Name) + "," + nameof(TenantId), true)]
    [Index("idx_{tablename}_02", nameof(Code) + "," + nameof(TenantId), true)]
    public partial class RoleEntity : EntityFull, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = 2, CanUpdate = false)]
        public long? TenantId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Column(StringLength = 50)]
        public string Code { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Column(StringLength = 200)]
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
        public ICollection<UserEntity> Users { get; set; }

        [Navigate(ManyToMany = typeof(RolePermissionEntity))]
        public ICollection<PermissionEntity> Permissions { get; set; }
    }
}