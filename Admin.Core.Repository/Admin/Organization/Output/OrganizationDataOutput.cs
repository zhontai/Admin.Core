using System.Collections.Generic;

namespace Admin.Core.Repository.Admin.Output
{
    /// <summary>
    /// 组织机构数据导出
    /// </summary>
    public class OrganizationDataOutput
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public long? TenantId { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 接口父级
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 值
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

        public List<OrganizationDataOutput> Childs { get; set; }
    }
}