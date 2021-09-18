using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.LoginLog.Input;
using System.Threading.Tasks;

namespace My.Admin.Service.Admin.LoginLog
{
    public interface ILoginLogService
    {
        Task<IResponseOutput> PageAsync(PageInput<LoginLogEntity> input);

        Task<IResponseOutput<long>> AddAsync(LoginLogAddInput input);
    }
}