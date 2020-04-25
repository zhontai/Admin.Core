using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Core.Model.Output;
using Admin.Core.Service.Admin.Document;
using Admin.Core.Service.Admin.Document.Input;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 文档管理
    /// </summary>
    public class DocumentController : AreaController
    {
        private readonly IDocumentService _documentServices;

        public DocumentController(IDocumentService documentServices)
        {
            _documentServices = documentServices;
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
    }
}
