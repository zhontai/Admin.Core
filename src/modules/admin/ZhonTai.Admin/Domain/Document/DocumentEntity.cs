using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;

namespace ZhonTai.Admin.Domain.Document;

/// <summary>
/// 文档
/// </summary>
[Table(Name = "ad_document")]
[Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Label) + "," + nameof(TenantId), true)]
public partial class DocumentEntity : EntityTenant
{
    /// <summary>
    /// 父级节点
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Label { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [Column(MapType = typeof(int), CanUpdate = false)]
    public DocumentType Type { get; set; }

    /// <summary>
    /// 命名
    /// </summary>
    [Column(StringLength = 500)]
    public string Name { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [Column(StringLength = -1)]
    public string Content { get; set; }

    /// <summary>
    /// Html
    /// </summary>
    [Column(StringLength = -1)]
    public string Html { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// 打开组
    /// </summary>
    public bool? Opened { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? Sort { get; set; } = 0;

    /// <summary>
    /// 描述
    /// </summary>
    [Column(StringLength = 100)]
    public string Description { get; set; }
}