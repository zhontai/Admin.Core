using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using ZhonTai.Admin.Domain.Organization;
using ZhonTai.Admin.Domain.Position;

namespace ZhonTai.Admin.Domain.Employee
{
    /// <summary>
    /// 员工
    /// </summary>
	[Table(Name = "ad_employee")]
    [Index("idx_{tablename}_01", nameof(Code) + "," + nameof(TenantId), true)]
    public partial class EmployeeEntity : EntityFull, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10)]
        public long? TenantId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column(StringLength = 20)]
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Column(StringLength = 20)]
        public string NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public SexEnum? Sex { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Column(StringLength = 20)]
        public string Code { get; set; }

        /// <summary>
        /// 主属部门Id
        /// </summary>
        public long OrganizationId { get; set; }

        public OrganizationEntity Organization { get; set; }

        /// <summary>
        /// 职位Id
        /// </summary>
        public long PositionId { get; set; }

        public PositionEntity Position { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Column(StringLength = 20)]
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column(StringLength = 250)]
        public string Email { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? EntryTime { get; set; }

        [Navigate(ManyToMany = typeof(EmployeeOrganizationEntity))]
        public ICollection<OrganizationEntity> Organizations { get; set; }
    }
}