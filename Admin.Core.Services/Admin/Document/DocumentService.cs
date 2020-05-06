using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Admin.Core.Repository.Admin;
using Admin.Core.Model.Admin;
using Admin.Core.Common.Output;
using Admin.Core.Service.Admin.Document.Input;
using Admin.Core.Service.Admin.Document.Output;
using Admin.Core.Common.Cache;


namespace Admin.Core.Service.Admin.Document
{	
	public class DocumentService : IDocumentService
    {
        private readonly IMapper _mapper;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentImageRepository _documentImageRepository;

        public DocumentService(
            IMapper mapper,
            IDocumentRepository DocumentRepository,
            IDocumentImageRepository documentImageRepository
        )
        {
            _mapper = mapper;
            _documentRepository = DocumentRepository;
            _documentImageRepository = documentImageRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var result = await _documentRepository.GetAsync(id);

            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> GetGroupAsync(long id)
        {
            var result = await _documentRepository.GetAsync<DocumentGetGroupOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> GetMenuAsync(long id)
        {
            var result = await _documentRepository.GetAsync<DocumentGetMenuOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> GetContentAsync(long id)
        {
            var result = await _documentRepository.GetAsync<DocumentGetContentOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> GetListAsync(string key, DateTime? start, DateTime? end)
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

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> GetImageListAsync(long id)
        {
            var result = await _documentImageRepository.Select
                .Where(a => a.DocumentId == id)
                .OrderByDescending(a=>a.Id)
                .ToListAsync(a => a.Url);

            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> AddGroupAsync(DocumentAddGroupInput input)
        {
            var entity = _mapper.Map<DocumentEntity>(input);
            var id = (await _documentRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id > 0);
        }

        public async Task<IResponseOutput> AddMenuAsync(DocumentAddMenuInput input)
        {
            var entity = _mapper.Map<DocumentEntity>(input);
            var id = (await _documentRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id > 0);
        }

        public async Task<IResponseOutput> AddImageAsync(DocumentAddImageInput input)
        {
            var entity = _mapper.Map<DocumentImageEntity>(input);
            var id = (await _documentImageRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id > 0);
        }

        public async Task<IResponseOutput> UpdateGroupAsync(DocumentUpdateGroupInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _documentRepository.GetAsync(input.Id);
                entity = _mapper.Map(input, entity);
                result = (await _documentRepository.UpdateAsync(entity)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> UpdateMenuAsync(DocumentUpdateMenuInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _documentRepository.GetAsync(input.Id);
                entity = _mapper.Map(input, entity);
                result = (await _documentRepository.UpdateAsync(entity)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> UpdateContentAsync(DocumentUpdateContentInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _documentRepository.GetAsync(input.Id);
                entity = _mapper.Map(input, entity);
                result = (await _documentRepository.UpdateAsync(entity)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _documentRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> DeleteImageAsync(long documentId, string url)
        {
            var result = false;
            if (documentId > 0 && url.NotNull())
            {
                result = (await _documentImageRepository.DeleteAsync(m => m.DocumentId == documentId && m.Url == url)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(long id)
        {
            var result = await _documentRepository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> GetPlainListAsync()
        {
            var documents = await _documentRepository.Select
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync(a => new { a.Id, a.ParentId, a.Label, a.Type, a.Opened });

            var menus = documents
                .Where(a => (new[] { DocumentType.Group, DocumentType.Markdown }).Contains(a.Type))
                .Select(a => new
                {
                    a.Id,
                    a.ParentId,
                    a.Label,
                    a.Type,
                    a.Opened
                });

            return ResponseOutput.Ok(menus);
        }
    }
}
