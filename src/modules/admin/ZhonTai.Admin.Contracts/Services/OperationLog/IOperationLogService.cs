using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.OperationLog.Dto;

namespace ZhonTai.Admin.Services.OperationLog;

/// <summary>
/// 操作日志接口
/// </summary>
public interface IOperationLogService
{
    Task<PageOutput<OperationLogGetPageOutput>> GetPageAsync(PageInput<OperationLogGetPageInput> input);

    Task<long> AddAsync(OperationLogAddInput input);
}