using System;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Service.Document.Dto;

namespace ZhonTai.Plate.Admin.Service.Document
{
    public partial interface IDocumentService
    {
        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetImageListAsync(long id);

        Task<IResultOutput> GetGroupAsync(long id);

        Task<IResultOutput> GetMenuAsync(long id);

        Task<IResultOutput> GetContentAsync(long id);

        Task<IResultOutput> GetPlainListAsync();

        Task<IResultOutput> GetListAsync(string key, DateTime? start, DateTime? end);

        Task<IResultOutput> AddGroupAsync(DocumentAddGroupInput input);

        Task<IResultOutput> AddMenuAsync(DocumentAddMenuInput input);

        Task<IResultOutput> AddImageAsync(DocumentAddImageInput input);

        Task<IResultOutput> UpdateGroupAsync(DocumentUpdateGroupInput input);

        Task<IResultOutput> UpdateMenuAsync(DocumentUpdateMenuInput input);

        Task<IResultOutput> UpdateContentAsync(DocumentUpdateContentInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> DeleteImageAsync(long documentId, string url);

        Task<IResultOutput> SoftDeleteAsync(long id);
    }
}