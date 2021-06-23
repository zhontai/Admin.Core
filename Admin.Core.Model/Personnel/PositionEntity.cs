using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;
using System;

namespace Admin.Core.Model.Personnel
{
    /// <summary>
    /// 职位
    /// </summary>
	[Table(Name = "pe_position")]
    [Index("idx_{tablename}_01", nameof(Name) + "," + nameof(TenantId), true)]
    [Index("idx_{tablename}_02", nameof(Code) + "," + nameof(TenantId), true)]
    public class PositionEntity : EntityFull, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10, CanUpdate = false)]
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
    }
}