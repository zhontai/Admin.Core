using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.OprationLog.Input;

namespace Admin.Core.Service.Admin.OprationLog
{	
    public interface IOprationLogService
    {
        Task<IResponseOutput> PageAsync(PageInput<OprationLogEntity> input);

        Task<IResponseOutput> AddAsync(OprationLogAddInput input);
    }
}
