using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.OprationLog.Input;
using System.Threading.Tasks;

namespace My.Admin.Service.Admin.OprationLog
{
    public interface IOprationLogService
    {
        Task<IResponseOutput> PageAsync(PageInput<OprationLogEntity> input);

        Task<IResponseOutput> AddAsync(OprationLogAddInput input);
    }
}