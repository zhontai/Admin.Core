using System.Collections.Generic;

namespace Admin.Core.Repository.Admin.View.Output
{
    public class ViewDataOutput
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public long? TenantId { get; set; }

        /// <summary>
        /// 视图Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 视图父级
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 视图命名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 视图路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        public List<ViewDataOutput> Childs { get; set; }
    }
}