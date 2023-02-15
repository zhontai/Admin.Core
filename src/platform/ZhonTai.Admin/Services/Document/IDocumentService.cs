using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.Document.Dto;

namespace ZhonTai.Admin.Services.Document;

/// <summary>
/// 文档接口
/// </summary>
public partial interface IDocumentService
{
    Task<List<string>> GetImageListAsync(long id);

    Task<DocumentGetGroupOutput> GetGroupAsync(long id);

    Task<DocumentGetMenuOutput> GetMenuAsync(long id);

    Task<DocumentGetContentOutput> GetContentAsync(long id);

    Task<IEnumerable<dynamic>> GetPlainListAsync();

    Task<List<DocumentListOutput>> GetListAsync(string key, DateTime? start, DateTime? end);

    Task<long> AddGroupAsync(DocumentAddGroupInput input);

    Task<long> AddMenuAsync(DocumentAddMenuInput input);

    Task<long> AddImageAsync(DocumentAddImageInput input);

    Task UpdateGroupAsync(DocumentUpdateGroupInput input);

    Task UpdateMenuAsync(DocumentUpdateMenuInput input);

    Task UpdateContentAsync(DocumentUpdateContentInput input);

    Task DeleteAsync(long id);

    Task DeleteImageAsync(long documentId, string url);

    Task SoftDeleteAsync(long id);

    Task<string> UploadImage([FromForm] DocumentUploadImageInput input);
}