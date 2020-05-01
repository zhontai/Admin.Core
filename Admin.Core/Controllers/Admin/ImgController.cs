using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Admin.Core.Common.Output;
using Admin.Core.Attributes;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Auth;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 图片管理
    /// </summary>
    public class ImgController : AreaController
    {
        private readonly IUser _user;
        private readonly UploadConfig _uploadConfig;
        private readonly UploadHelper _uploadHelper;

        public ImgController(
            IUser user, 
            IOptionsMonitor<UploadConfig> uploadConfig, 
            UploadHelper uploadHelper
        )
        {
            _user = user;
            _uploadConfig = uploadConfig.CurrentValue;
            _uploadHelper = uploadHelper;
        }

        /*
        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{fileName}")]
        [NoOprationLog]
        [AllowAnonymous]
        public FileStreamResult Avatar([FromServices]IWebHostEnvironment environment, string fileName = "")
        {
            string filePath = Path.Combine(environment.WebRootPath,"avatar", fileName).ToPath();
            var stream = System.IO.File.OpenRead(filePath);
            string fileExt = Path.GetExtension(filePath);
            var contentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            var contentType = contentTypeProvider.Mappings[fileExt];
            var fileDownloadName = Path.GetFileName(filePath);

            return File(stream, contentType, fileDownloadName);
        }
        */

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Login]
        public async Task<IResponseOutput> AvatarUpload([FromForm]IFormFile file)
        {
            var config = _uploadConfig.Avatar;
            var res = await _uploadHelper.UploadAsync(file, config, new { _user.Id });
            if (res.Success)
            {
                return ResponseOutput.Ok(res.Data.FileRelativePath);
            }

            return ResponseOutput.NotOk("上传失败！");
        }
    }
}
