using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 文档图片
    /// </summary>
	[Table(Name = "ad_document_image")]
    [Index("uk_document_image_documentid_url", nameof(DocumentId) + "," + nameof(Url), true)]
    public class DocumentImageEntity: EntityAdd
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long DocumentId { get; set; }
        
        public DocumentEntity Document { get; set; }
        
        /// <summary>
        /// 请求路径
        /// </summary>
        public string Url { get; set; }
    }
}
