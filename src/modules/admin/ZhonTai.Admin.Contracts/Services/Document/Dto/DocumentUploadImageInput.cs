using Microsoft.AspNetCore.Http;

namespace ZhonTai.Admin.Services.Document.Dto;

public class DocumentUploadImageInput
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public IFormFile File { get; set; }

    /// <summary>
    /// 文档编号
    /// </summary>
    public long Id { get; set; }
}