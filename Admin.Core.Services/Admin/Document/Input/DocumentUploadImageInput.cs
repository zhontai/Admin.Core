using Microsoft.AspNetCore.Http;

namespace Admin.Core.Service.Admin.Document.Input
{
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
}
