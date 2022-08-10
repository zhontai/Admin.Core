using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using ZhonTai.Admin.Domain.Organization;

namespace ZhonTai.Admin.Domain.Employee
{
    /// <summary>
    /// 员工
    /// </summary>
	[Table(Name = "ad_employee")]
    [Index("idx_{tablename}_01", nameof(JobNumber) + "," + nameof(TenantId), true)]
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
        /// 性别
        /// </summary>
        public SexEnum? Sex { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Column(StringLength = 20)]
        public string JobNumber { get; set; }

        /// <summary>
        /// 主属部门Id
        /// </summary>
        public long MainOrgId { get; set; }

        public OrganizationEntity MainOrg { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? EntryTime { get; set; }

        [Navigate(ManyToMany = typeof(EmployeeOrganizationEntity))]
        public ICollection<OrganizationEntity> Orgs { get; set; }
    }
}