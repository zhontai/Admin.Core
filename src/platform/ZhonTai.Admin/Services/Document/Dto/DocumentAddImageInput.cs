namespace ZhonTai.Admin.Services.Document.Dto;

public class DocumentAddImageInput
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