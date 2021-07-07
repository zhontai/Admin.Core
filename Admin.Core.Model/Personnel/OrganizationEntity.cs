using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;
using System.Collections.Generic;

namespace Admin.Core.Model.Personnel
{
    /// <summary>
    /// 组织架构
    /// </summary>
	[Table(Name = "pe_organization")]
    [Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Name) + "," + nameof(TenantId), true)]
    public class OrganizationEntity : EntityFull, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10, CanUpdate = false)]
        public long? TenantId { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
		public long ParentId { get; set; }

        [Navigate(nameof(ParentId))]
        public List<OrganizationEntity> Childs { get; set; }

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
        /// 值
        /// </summary>
        [Column(StringLength = 50)]
        public string Value { get; set; }

        /// <summary>
        /// 主管Id
        /// </summary>
        public long? PrimaryEmployeeId { get; set; }

        /// <summary>
        /// 主管
        /// </summary>
        public EmployeeEntity PrimaryEmployee { get; set; }

        /// <summary>
        /// 员工人数
        /// </summary>
        public int EmployeeCount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column(StringLength = 500)]
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;

        /// <summary>
        /// 排序
        /// </summary>
		public int Sort { get; set; }

        [Navigate(ManyToMany = typeof(EmployeeOrganizationEntity))]
        public ICollection<EmployeeEntity> Employees { get; set; }
    }
}