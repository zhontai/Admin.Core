using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.File.Dto;
using ZhonTai.Admin.Services.File.Dto;

namespace ZhonTai.Admin.Services.File;

/// <summary>
/// 文件接口
/// </summary>
public interface IFileService
{
    Task<PageOutput<FileGetPageOutput>> GetPageAsync(PageInput<FileGetPageDto> input);
}