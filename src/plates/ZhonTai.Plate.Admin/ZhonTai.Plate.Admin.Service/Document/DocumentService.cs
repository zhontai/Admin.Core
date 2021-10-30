using System;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain.Document;
using ZhonTai.Plate.Admin.Domain.DocumentImage;
using ZhonTai.Plate.Admin.Service.Document.Output;
using ZhonTai.Plate.Admin.Service.Document.Input;

namespace ZhonTai.Plate.Admin.Service.Document
{
    public class DocumentService : BaseService, IDocumentService
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

        public async Task<IResultOutput> GetAsync(long id)
        {
            var result = await _documentRepository.GetAsync(id);

            return ResultOutput.Ok(result);
        }

        public async Task<IResultOutput> GetGroupAsync(long id)
        {
            var result = await _documentRepository.GetAsync<DocumentGetGroupOutput>(id);
            return ResultOutput.Ok(result);
        }

        public async Task<IResultOutput> GetMenuAsync(long id)
        {
            var result = await _documentRepository.GetAsync<DocumentGetMenuOutput>(id);
            return ResultOutput.Ok(result);
        }

        public async Task<IResultOutput> GetContentAsync(long id)
        {
            var result = await _documentRepository.GetAsync<DocumentGetContentOutput>(id);
            return ResultOutput.Ok(result);
        }

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

        public async Task<IResultOutput> GetImageListAsync(long id)
        {
            var result = await _documentImageRepository.Select
                .Where(a => a.DocumentId == id)
                .OrderByDescending(a => a.Id)
                .ToListAsync(a => a.Url);

            return ResultOutput.Ok(result);
        }

        public async Task<IResultOutput> AddGroupAsync(DocumentAddGroupInput input)
        {
            var entity = Mapper.Map<DocumentEntity>(input);
            var id = (await _documentRepository.InsertAsync(entity)).Id;

            return ResultOutput.Result(id > 0);
        }

        public async Task<IResultOutput> AddMenuAsync(DocumentAddMenuInput input)
        {
            var entity = Mapper.Map<DocumentEntity>(input);
            var id = (await _documentRepository.InsertAsync(entity)).Id;

            return ResultOutput.Result(id > 0);
        }

        public async Task<IResultOutput> AddImageAsync(DocumentAddImageInput input)
        {
            var entity = Mapper.Map<DocumentImageEntity>(input);
            var id = (await _documentImageRepository.InsertAsync(entity)).Id;

            return ResultOutput.Result(id > 0);
        }

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

        public async Task<IResultOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _documentRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResultOutput.Result(result);
        }

        public async Task<IResultOutput> DeleteImageAsync(long documentId, string url)
        {
            var result = false;
            if (documentId > 0 && url.NotNull())
            {
                result = (await _documentImageRepository.DeleteAsync(m => m.DocumentId == documentId && m.Url == url)) > 0;
            }

            return ResultOutput.Result(result);
        }

        public async Task<IResultOutput> SoftDeleteAsync(long id)
        {
            var result = await _documentRepository.SoftDeleteAsync(id);
            return ResultOutput.Result(result);
        }

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
    }
}