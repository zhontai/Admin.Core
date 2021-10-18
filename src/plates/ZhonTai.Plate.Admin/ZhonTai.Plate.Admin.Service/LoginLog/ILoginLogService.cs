using ZhonTai.Common.Input;
using ZhonTai.Common.Output;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.Domain.LoginLog;
using ZhonTai.Plate.Admin.Service.LoginLog.Input;

namespace ZhonTai.Plate.Admin.Service.LoginLog
{
    public interface ILoginLogService
    {
        Task<IResponseOutput> PageAsync(PageInput<LoginLogEntity> input);

        Task<IResponseOutput<long>> AddAsync(LoginLogAddInput input);
    }
}