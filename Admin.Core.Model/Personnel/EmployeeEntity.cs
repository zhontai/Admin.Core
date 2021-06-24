using Admin.Core.Common.BaseModel;
using Admin.Core.Model.Admin;
using Admin.Core.Model.Enums;
using FreeSql.DataAnnotations;
using System;

namespace Admin.Core.Model.Personnel
{
    /// <summary>
    /// 员工
    /// </summary>
	[Table(Name = "pe_employee")]
    [Index("idx_{tablename}_01", nameof(Code) + "," + nameof(TenantId), true)]
    public class EmployeeEntity : EntityFull, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10)]
        public long? TenantId { get; set; }

        public TenantEntity Tenant { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public long OrganizationId { get; set; }

        public OrganizationEntity Organization { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Column(StringLength = 20)]
        public string Phone { get; set; }

        /// <summary>
        /// 职位Id
        /// </summary>
        public long PositionId { get; set; }

        public PositionEntity Position { get; set; }
    }
}