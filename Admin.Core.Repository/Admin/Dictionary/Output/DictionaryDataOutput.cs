using System.Collections.Generic;

namespace Admin.Core.Repository.Admin.Output
{
    /// <summary>
    /// 数据字典导出
    /// </summary>
    public partial class DictionaryDataOutput
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public long? TenantId { get; set; }

        /// <summary>
        /// 字典Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 字典类型Id
        /// </summary>
        public long DictionaryTypeId { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字典编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 字典值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
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