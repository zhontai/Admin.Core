namespace ZhonTai.Admin.Services.Document.Dto;

public class DocumentGetContentOutput
{
    /// <summary>
    /// 编号
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
}