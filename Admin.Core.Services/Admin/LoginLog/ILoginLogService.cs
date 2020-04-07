using System.Threading.Tasks;
using Admin.Core.Model.Input;
using Admin.Core.Model.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.LoginLog.Input;

namespace Admin.Core.Service.Admin.LoginLog
{	
    public interface ILoginLogService
    {
        Task<IResponseOutput> PageAsync(PageInput<LoginLogEntity> input);

        Task<IResponseOutput<long>> AddAsync(LoginLogAddInput input);
    }
}
