using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.OprationLog.Dto;
using ZhonTai.Admin.Domain;

namespace ZhonTai.Admin.Services.OprationLog
{
    /// <summary>
    /// 操作日志接口
    /// </summary>
    public interface IOprationLogService
    {
        Task<IResultOutput> GetPageAsync(PageInput<LogGetPageDto> input);

        Task<IResultOutput> AddAsync(OprationLogAddInput input);
    }
}