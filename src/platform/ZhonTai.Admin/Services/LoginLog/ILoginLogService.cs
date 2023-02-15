using ZhonTai.Admin.Core.Dto;
using System.Threading.Tasks;
using ZhonTai.Admin.Services.LoginLog.Dto;
using ZhonTai.Admin.Domain;

namespace ZhonTai.Admin.Services.LoginLog;

/// <summary>
/// 登录日志接口
/// </summary>
public interface ILoginLogService
{
    Task<PageOutput<LoginLogListOutput>> GetPageAsync(PageInput<LogGetPageDto> input);

    Task<long> AddAsync(LoginLogAddInput input);
}