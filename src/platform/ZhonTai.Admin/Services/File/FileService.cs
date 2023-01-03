using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.File.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Domain.File.Dto;
using ZhonTai.Admin.Domain.File;

namespace ZhonTai.Admin.Services.File;

/// <summary>
/// 文件服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class FileService : BaseService, IFileService, IDynamicApi
{
    private IFileRepository _fileRepository => LazyGetRequiredService<IFileRepository>();

    public FileService()
    {

    }

    /// <summary>
    /// 查询文件列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<FileGetPageOutput>> GetPageAsync(PageInput<FileGetPageDto> input)
    {
        var fileName = input.Filter?.FileName;

        var list = await _fileRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(fileName.NotNull(), a => a.FileName.Contains(fileName))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<FileGetPageOutput>();

        var data = new PageOutput<FileGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }
}