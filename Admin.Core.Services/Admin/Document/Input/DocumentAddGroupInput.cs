using Admin.Core.Model.Admin;

namespace Admin.Core.Service.Admin.Document.Input
{
    public class DocumentAddGroupInput
    {
        /// <summary>
        /// 父级节点
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public DocumentType Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 命名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 打开
        /// </summary>
        public bool? Opened { get; set; }
    }
}
