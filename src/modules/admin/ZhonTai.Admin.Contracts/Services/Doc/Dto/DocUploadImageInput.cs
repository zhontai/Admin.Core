using Microsoft.AspNetCore.Http;

namespace ZhonTai.Admin.Services.Doc.Dto;

/// <summary>
/// 上传图片
/// </summary>
public class DocUploadImageInput
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