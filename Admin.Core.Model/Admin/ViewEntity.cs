using System;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 视图管理
    /// </summary>
	[Table(Name = "ad_view")]
    [Index("uk_view_parentid_label", nameof(ParentId) + "," + nameof(Label), true)]
    public class ViewEntity : EntityBase
    {
        /// <summary>
        /// 所属节点
        /// </summary>
		public long ParentId { get; set; }

        /// <summary>
        /// 视图命名
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        [Column(StringLength = 500)]
        public string Label { get; set; }

        /// <summary>
        /// 视图路径
        /// </summary>
        [Column(StringLength = 500)]
        public string Path { get; set; }

        /// <summary>
        /// 说明
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
    }
}
