using System;
using Admin.Core.Common.BaseModel;
using FreeSql;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 租户
    /// </summary>
	[Table(Name = "ad_tenant")]
    [Index("idx_{tablename}_01", nameof(Name), true)]
    [Index("idx_{tablename}_02", nameof(Code), true)]
    public class TenantEntity : EntityFull, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10, CanUpdate = false)]
        public long? TenantId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Column(StringLength = 50)]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        [Column(MapType = typeof(int))]
        public DataType? DbType { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        [Column(StringLength = 500)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 空闲时间(分)
        /// </summary>
        public int? IdleTime { get; set; } = 10;

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;

        /// <summary>
        /// 说明
        /// </summary>
        [Column(StringLength = 500)]
        public string Description { get; set; }
    }

}
