using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Domain.Doc;
using ZhonTai.Admin.Domain.DocImage;
using ZhonTai.Admin.Services.Doc.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;

namespace ZhonTai.Admin.Services.Doc;

/// <summary>
/// 文档服务
/// </summary>
[Order(180)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DocService : BaseService, IDocService, IDynamicApi
{
    private readonly IDocRepository _docRep;
    private readonly IDocImageRepository _docImageRep;
    private readonly Lazy<IFileService> _fileService;

    public DocService(
        IDocRepository docRep,
        IDocImageRepository docImageRep,
        Lazy<IFileService> fileService
    )
    {
        _docRep = docRep;
        _docImageRep = docImageRep;
        _fileService = fileService;
    }

    /// <summary>
    /// 查询分组
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DocGetGroupOutput> GetGroupAsync(long id)
    {
        var result = await _docRep.GetAsync<DocGetGroupOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询菜单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DocGetMenuOutput> GetMenuAsync(long id)
    {
        var result = await _docRep.GetAsync<DocGetMenuOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询文档内容
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DocGetContentOutput> GetContentAsync(long id)
    {
        var result = await _docRep.GetAsync<DocGetContentOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询文档列表
    /// </summary>
    /// <param name="key"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public async Task<List<DocListOutput>> GetListAsync(string key, DateTime? start, DateTime? end)
    {
        if (end.HasValue)
        {
            end = end.Value.AddDays(1);
        }

        var data = await _docRep
            .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Label.Contains(key))
            .WhereIf(start.HasValue && end.HasValue, a => a.CreatedTime.Value.BetweenEnd(start.Value, end.Value))
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync<DocListOutput>();

        return data;
    }

    /// <summary>
    /// 查询图片列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<List<string>> GetImageListAsync(long id)
    {
        var result = await _docImageRep.Select
            .Where(a => a.DocumentId == id)
            .OrderByDescending(a => a.Id)
            .ToListAsync(a => a.Url);

        return result;
    }

    /// <summary>
    /// 新增分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddGroupAsync(DocAddGroupInput input)
    {
        var entity = Mapper.Map<DocEntity>(input);
        await _docRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddMenuAsync(DocAddMenuInput input)
    {
        var entity = Mapper.Map<DocEntity>(input);
        await _docRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 新增图片
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddImageAsync(DocAddImageInput input)
    {
        var entity = Mapper.Map<DocImageEntity>(input);
        await _docImageRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateGroupAsync(DocUpdateGroupInput input)
    {
        var entity = await _docRep.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        await _docRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateMenuAsync(DocUpdateMenuInput input)
    {
        var entity = await _docRep.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        await _docRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改文档内容
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateContentAsync(DocUpdateContentInput input)
    {
        var entity = await _docRep.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        await _docRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除文档
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _docRep.DeleteAsync(m => m.Id == id);
    }

    /// <summary>
    /// 彻底删除图片
    /// </summary>
    /// <param name="documentId"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task DeleteImageAsync(long documentId, string url)
    {
        await _docImageRep.DeleteAsync(m => m.DocumentId == documentId && m.Url == url);
    }

    /// <summary>
    /// 删除文档
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        await _docRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 查询精简文档列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<dynamic>> GetPlainListAsync()
    {
        var documents = await _docRep.Select
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync(a => new { a.Id, a.ParentId, a.Label, a.Type, a.Opened });

        var menus = documents
            .Where(a => (new[] { DocType.Group, DocType.Markdown }).Contains(a.Type))
            .Select(a => new
            {
                a.Id,
                a.ParentId,
                a.Label,
                a.Type,
                a.Opened
            });

        return menus;
    }

    /// <summary>
    /// 上传文档图片
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> UploadImage([FromForm] DocUploadImageInput input)
    {
        var fileInfo = await _fileService.Value.UploadFileAsync(input.File);
        //保存文档图片
        await AddImageAsync(new DocAddImageInput
        {
            DocumentId = input.Id,
            Url = fileInfo.LinkUrl
        });
        return fileInfo.LinkUrl;
    }
}