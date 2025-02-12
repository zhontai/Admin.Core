namespace ZhonTai.Admin.Services.Doc.Dto;

/// <summary>
/// 添加图片
/// </summary>
public class DocAddImageInput
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long DocumentId { get; set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    public string Url { get; set; }
}