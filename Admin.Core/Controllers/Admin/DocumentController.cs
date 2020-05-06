using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Core.Common.Output;
using Admin.Core.Service.Admin.Document;
using Admin.Core.Service.Admin.Document.Input;
using Admin.Core.Common.Configs;
using Microsoft.Extensions.Options;
using Admin.Core.Common.Helpers;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 文档管理
    /// </summary>
    public class DocumentController : AreaController
    {
        private readonly IDocumentService _documentServices;
        private readonly UploadConfig _uploadConfig;
        private readonly UploadHelper _uploadHelper;

        public DocumentController(
            IDocumentService documentServices, 
            IOptionsMonitor<UploadConfig> uploadConfig,
            UploadHelper uploadHelper
        )
        {
            _documentServices = documentServices;
            _uploadConfig = uploadConfig.CurrentValue;
            _uploadHelper = uploadHelper;
        }

        /// <summary>
        /// 查询文档列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetList(string key, DateTime? start, DateTime? end)
        {
            return await _documentServices.GetListAsync(key,start,end);
        }

        /// <summary>
        /// 查询文档图片列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetImageList(long id)
        {
            return await _documentServices.GetImageListAsync(id);
        }

        /// <summary>
        /// 查询单条分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetGroup(long id)
        {
            return await _documentServices.GetGroupAsync(id);
        }

        /// <summary>
        /// 查询单条菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetMenu(long id)
        {
            return await _documentServices.GetMenuAsync(id);
        }

        /// <summary>
        /// 查询单条文档内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetContent(long id)
        {
            return await _documentServices.GetContentAsync(id);
        }

        /// <summary>
        /// 查询精简文档列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPlainList()
        {
            return await _documentServices.GetPlainListAsync();
        }

        /// <summary>
        /// 新增分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> AddGroup(DocumentAddGroupInput input)
        {
            return await _documentServices.AddGroupAsync(input);
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> AddMenu(DocumentAddMenuInput input)
        {
            return await _documentServices.AddMenuAsync(input);
        }

        /// <summary>
        /// 修改分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateGroup(DocumentUpdateGroupInput input)
        {
            return await _documentServices.UpdateGroupAsync(input);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateMenu(DocumentUpdateMenuInput input)
        {
            return await _documentServices.UpdateMenuAsync(input);
        }

        /// <summary>
        /// 修改文档内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateContent(DocumentUpdateContentInput input)
        {
            return await _documentServices.UpdateContentAsync(input);
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _documentServices.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> DeleteImage(long documentId, string url)
        {
            return await _documentServices.DeleteImageAsync(documentId, url);
        }

        /// <summary>
        /// 上传文档图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> UploadImage([FromForm]DocumentUploadImageInput input)
        {
            var config = _uploadConfig.Document;
            var res = await _uploadHelper.UploadAsync(input.File, config, new { input.Id });
            if (res.Success)
            {
                //保存文档图片
                var r = await _documentServices.AddImageAsync(
                new DocumentAddImageInput
                {
                    DocumentId = input.Id,
                    Url = res.Data.FileRequestPath
                });
                if (r.Success)
                {
                    return ResponseOutput.Ok(res.Data.FileRequestPath);
                }
            }

            return ResponseOutput.NotOk("上传失败！");
        }
    }
}
