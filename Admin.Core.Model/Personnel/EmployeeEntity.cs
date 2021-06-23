using Admin.Core.Common.BaseModel;
using Admin.Core.Model.Admin;
using FreeSql.DataAnnotations;
using System;

namespace Admin.Core.Model.Personnel
{
    /// <summary>
    /// 员工
    /// </summary>
	[Table(Name = "pe_employee")]
    [Index("idx_{tablename}_01", nameof(EmpNo) + "," + nameof(TenantId), true)]
    public class EmployeeEntity : EntityFull, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10)]
        public long? TenantId { get; set; }

        public TenantEntity Tenant { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string EmpNo { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public long OrganizationId { get; set; }

        public OrganizationEntity Organization { get; set; }
    }
}