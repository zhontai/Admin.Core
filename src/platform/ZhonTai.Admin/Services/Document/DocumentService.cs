using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Document;
using ZhonTai.Admin.Domain.DocumentImage;
using ZhonTai.Admin.Services.Document.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Core.Helpers;

namespace ZhonTai.Admin.Services.Document
{
    /// <summary>
    /// 文档服务
    /// </summary>
    [DynamicApi(Area = "admin")]
    public class DocumentService : BaseService, IDocumentService, IDynamicApi
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentImageRepository _documentImageRepository;

        public DocumentService(
            IDocumentRepository DocumentRepository,
            IDocumentImageRepository documentImageRepository
        )
        {
            _documentRepository = DocumentRepository;
            _documentImageRepository = documentImageRepository;
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> GetAsync(long id)
        {
            var result = await _documentRepository.GetAsync(id);

            return ResultOutput.Ok(result);
        }

        /// <summary>
        /// 查询分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> GetGroupAsync(long id)
        {
            var result = await _documentRepository.GetAsync<DocumentGetGroupOutput>(id);
            return ResultOutput.Ok(result);
        }

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> GetMenuAsync(long id)
        {
            var result = await _documentRepository.GetAsync<DocumentGetMenuOutput>(id);
            return ResultOutput.Ok(result);
        }

        /// <summary>
        /// 查询文档内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> GetContentAsync(long id)
        {
            var result = await _documentRepository.GetAsync<DocumentGetContentOutput>(id);
            return ResultOutput.Ok(result);
        }

        /// <summary>
        /// 查询文档列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<IResultOutput> GetListAsync(string key, DateTime? start, DateTime? end)
        {
            if (end.HasValue)
            {
                end = end.Value.AddDays(1);
            }

            var data = await _documentRepository
                .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Label.Contains(key))
                .WhereIf(start.HasValue && end.HasValue, a => a.CreatedTime.Value.BetweenEnd(start.Value, end.Value))
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync<DocumentListOutput>();

            return ResultOutput.Ok(data);
        }

        /// <summary>
        /// 查询图片列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> GetImageListAsync(long id)
        {
            var result = await _documentImageRepository.Select
                .Where(a => a.DocumentId == id)
                .OrderByDescending(a => a.Id)
                .ToListAsync(a => a.Url);

            return ResultOutput.Ok(result);
        }

        /// <summary>
        /// 新增分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput> AddGroupAsync(DocumentAddGroupInput input)
        {
            var entity = Mapper.Map<DocumentEntity>(input);
            var id = (await _documentRepository.InsertAsync(entity)).Id;

            return ResultOutput.Result(id > 0);
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput> AddMenuAsync(DocumentAddMenuInput input)
        {
            var entity = Mapper.Map<DocumentEntity>(input);
            var id = (await _documentRepository.InsertAsync(entity)).Id;

            return ResultOutput.Result(id > 0);
        }

        /// <summary>
        /// 新增图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput> AddImageAsync(DocumentAddImageInput input)
        {
            var entity = Mapper.Map<DocumentImageEntity>(input);
            var id = (await _documentImageRepository.InsertAsync(entity)).Id;

            return ResultOutput.Result(id > 0);
        }

        /// <summary>
        /// 修改分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput> UpdateGroupAsync(DocumentUpdateGroupInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _documentRepository.GetAsync(input.Id);
                entity = Mapper.Map(input, entity);
                result = (await _documentRepository.UpdateAsync(entity)) > 0;
            }

            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput> UpdateMenuAsync(DocumentUpdateMenuInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _documentRepository.GetAsync(input.Id);
                entity = Mapper.Map(input, entity);
                result = (await _documentRepository.UpdateAsync(entity)) > 0;
            }

            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 修改文档内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput> UpdateContentAsync(DocumentUpdateContentInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _documentRepository.GetAsync(input.Id);
                entity = Mapper.Map(input, entity);
                result = (await _documentRepository.UpdateAsync(entity)) > 0;
            }

            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 彻底删除文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _documentRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 彻底删除图片
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<IResultOutput> DeleteImageAsync(long documentId, string url)
        {
            var result = false;
            if (documentId > 0 && url.NotNull())
            {
                result = (await _documentImageRepository.DeleteAsync(m => m.DocumentId == documentId && m.Url == url)) > 0;
            }

            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> SoftDeleteAsync(long id)
        {
            var result = await _documentRepository.SoftDeleteAsync(id);
            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 查询精简文档列表
        /// </summary>
        /// <returns></returns>
        public async Task<IResultOutput> GetPlainListAsync()
        {
            var documents = await _documentRepository.Select
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync(a => new { a.Id, a.ParentId, a.Label, a.Type, a.Opened });

            var menus = documents
                .Where(a => (new[] { DocumentTypeEnum.Group, DocumentTypeEnum.Markdown }).Contains(a.Type))
                .Select(a => new
                {
                    a.Id,
                    a.ParentId,
                    a.Label,
                    a.Type,
                    a.Opened
                });

            return ResultOutput.Ok(menus);
        }

        /// <summary>
        /// 上传文档图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> UploadImage([FromForm] DocumentUploadImageInput input)
        {
            var uploadConfig = LazyGetRequiredService<IOptionsMonitor<UploadConfig>>().CurrentValue;
            var uploadHelper = LazyGetRequiredService<UploadHelper>();

            var config = uploadConfig.Document;
            var res = await uploadHelper.UploadAsync(input.File, config, new { input.Id });
            if (res.Success)
            {
                //保存文档图片
                var r = await AddImageAsync(
                new DocumentAddImageInput
                {
                    DocumentId = input.Id,
                    Url = res.Data.FileRequestPath
                });
                if (r.Success)
                {
                    return ResultOutput.Ok(res.Data.FileRequestPath);
                }
            }

            return ResultOutput.NotOk("上传失败！");
        }
    }
}