using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Domain.Document;

namespace ZhonTai.Admin.Domain.DocumentImage;

/// <summary>
/// 文档图片
/// </summary>
[Table(Name = "ad_document_image")]
[Index("idx_{tablename}_01", nameof(DocumentId) + "," + nameof(Url), true)]
public class DocumentImageEntity : EntityAdd
{
    /// <summary>
    /// 文档Id
    /// </summary>
    public long DocumentId { get; set; }

    public DocumentEntity Document { get; set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    public string Url { get; set; }
}