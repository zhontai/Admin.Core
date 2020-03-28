using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.Core.Model.Output;
using Admin.Core.Attributes;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 图片管理
    /// </summary>
    [Area("Admin")]
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    [NoOprationLog]
    public class ImgController : ControllerBase
    {
        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{fileName}")]
        public FileStreamResult Avatar([FromServices]IWebHostEnvironment environment, string fileName = "")
        {
            string filepath = Path.Combine(environment.WebRootPath,"avatar", fileName);
            var stream = System.IO.File.OpenRead(filepath);
            string fileExt = Path.GetExtension(filepath);
            var contentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            var contentType = contentTypeProvider.Mappings[fileExt];
            var fileDownloadName = Path.GetFileName(filepath);

            return File(stream, contentType, fileDownloadName);
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{fileName}")]
        public FileStreamResult Download([FromServices]IWebHostEnvironment environment,string fileName = "")
        {
            string filepath = Path.Combine(environment.WebRootPath, "images", fileName);
            var stream = System.IO.File.OpenRead(filepath);
            string fileExt = Path.GetExtension(filepath);
            var contentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            var contentType = contentTypeProvider.Mappings[fileExt];
            var fileDownloadName = Path.GetFileName(filepath);

            return File(stream, contentType, fileDownloadName);
        }

        /// <summary>
        /// 上传图片
        /// 支持多图片上传
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Upload([FromServices]IWebHostEnvironment environment)
        {
            string path = string.Empty;
            string foldername = "images";
            IFormFileCollection files = null;

            try
            {
                files = Request.Form.Files;
            }
            catch (Exception)
            {
                files = null;
            }

            if (files == null || !files.Any()) 
            {
                return ResponseOutput.NotOk("请选择上传的文件。");
            }

            //格式限制
            var allowType = new string[] { "image/jpg", "image/png", "image/jpeg" };

            string folderpath = Path.Combine(environment.WebRootPath, foldername);
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }

            if (files.Any(c => allowType.Contains(c.ContentType)))
            {
                if (files.Sum(c => c.Length) <= 1024 * 1024 * 4)
                {
                    //foreach (var file in files)
                    var file = files.FirstOrDefault();
                    string strpath = Path.Combine(foldername, DateTime.Now.ToString("MMddHHmmss") + file.FileName);
                    path = Path.Combine(environment.WebRootPath, strpath);

                    using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return ResponseOutput.Ok(strpath);
                }
                else
                {
                    return ResponseOutput.NotOk("图片过大");
                }
            }
            else
            {
                return ResponseOutput.NotOk("图片格式错误");
            }
        }
    }

}
