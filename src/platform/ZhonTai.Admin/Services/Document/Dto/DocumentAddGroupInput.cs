using ZhonTai.Admin.Domain.Document;

namespace ZhonTai.Admin.Services.Document.Dto;

public class DocumentAddGroupInput
{
    /// <summary>
    /// 父级节点
    /// </summary>
    public long ParentId { get; set; }

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