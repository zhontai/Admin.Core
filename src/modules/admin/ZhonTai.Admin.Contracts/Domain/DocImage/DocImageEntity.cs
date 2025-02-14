using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Domain.Doc;

namespace ZhonTai.Admin.Domain.DocImage;

/// <summary>
/// 文档图片
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "doc_image", OldName = DbConsts.TableOldNamePrefix + "document_image")]
[Index("idx_{tablename}_01", nameof(DocumentId) + "," + nameof(Url), true)]
public class DocImageEntity : EntityAdd
{
    /// <summary>
    /// 文档Id
    /// </summary>
    public long DocumentId { get; set; }

    public DocEntity Document { get; set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    public string Url { get; set; }
}