using ZhonTai.Common.Domain.Dto;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.Domain.LoginLog;
using ZhonTai.Plate.Admin.Service.LoginLog.Input;

namespace ZhonTai.Plate.Admin.Service.LoginLog
{
    public interface ILoginLogService
    {
        Task<IResultOutput> GetPageAsync(PageInput<LoginLogEntity> input);

        Task<IResultOutput<long>> AddAsync(LoginLogAddInput input);
    }
}