using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Services.Doc.Dto;

namespace ZhonTai.Admin.Services.Doc;

/// <summary>
/// 文档接口
/// </summary>
public partial interface IDocService
{
    Task<List<string>> GetImageListAsync(long id);

    Task<DocGetGroupOutput> GetGroupAsync(long id);

    Task<DocGetMenuOutput> GetMenuAsync(long id);

    Task<DocGetContentOutput> GetContentAsync(long id);

    Task<IEnumerable<dynamic>> GetPlainListAsync();

    Task<List<DocListOutput>> GetListAsync(string key, DateTime? start, DateTime? end);

    Task<long> AddGroupAsync(DocAddGroupInput input);

    Task<long> AddMenuAsync(DocAddMenuInput input);

    Task<long> AddImageAsync(DocAddImageInput input);

    Task UpdateGroupAsync(DocUpdateGroupInput input);

    Task UpdateMenuAsync(DocUpdateMenuInput input);

    Task UpdateContentAsync(DocUpdateContentInput input);

    Task DeleteAsync(long id);

    Task DeleteImageAsync(long documentId, string url);

    Task SoftDeleteAsync(long id);

    Task<string> UploadImage([FromForm] DocUploadImageInput input);
}