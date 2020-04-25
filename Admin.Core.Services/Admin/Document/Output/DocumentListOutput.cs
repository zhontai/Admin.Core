using Admin.Core.Model.Admin;

namespace Admin.Core.Service.Admin.Document.Output
{
    public class DocumentListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父级节点
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public DocumentType Type { get; set; }

        /// <summary>
        /// 命名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 组打开
        /// </summary>
        public bool? Opened { get; set; }
    }
}