using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Admin.Core.Model.Output;
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
        
        public ImgController(IUser user, IOptionsMonitor<UploadConfig> uploadConfig)
        {
            _user = user;
            _uploadConfig = uploadConfig.CurrentValue;
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
            string filePath = Path.Combine(environment.WebRootPath,"avatar", fileName);
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
        /// <param name="environment"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Login]
        public async Task<IResponseOutput> AvatarUpload(IFormFile file)
        {
            if(file == null || file.Length < 1)
            {
                if (Request.Form.Files != null && Request.Form.Files.Any())
                {
                    file = Request.Form.Files[0];
                }
            }

            if (file == null || file.Length < 1)
            {
                return ResponseOutput.NotOk("请上传头像！");
            }

            var avatar = _uploadConfig.Avatar;

            //格式限制
            if (!avatar.ContentType.Contains(file.ContentType))
            {
                return ResponseOutput.NotOk("图片格式错误");
            }

            //大小限制
            if (!(file.Length <= avatar.Size))
            {
                return ResponseOutput.NotOk("图片过大");
            }

            var dateTimeFormat = avatar.DateTimeFormat.NotNull() ? DateTime.Now.ToString(avatar.DateTimeFormat) : "";
            var format = avatar.Format.NotNull() ? string.Format(avatar.Format,_user.Id) : "";
            var savePath = Path.Combine(dateTimeFormat, format);
            var fullDirectory = Path.Combine(avatar.Path, savePath);
            if (!Directory.Exists(fullDirectory))
            {
                Directory.CreateDirectory(fullDirectory);
            }

            var saveFileName = $"{new Snowfake(0).nextId()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(fullDirectory, saveFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return ResponseOutput.Ok(Path.Combine(savePath, saveFileName));
        }
    }
}
