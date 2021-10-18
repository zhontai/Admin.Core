using System.Threading.Tasks;
using ZhonTai.Common.Input;
using ZhonTai.Common.Output;
using ZhonTai.Plate.Admin.Service.OprationLog.Input;
using ZhonTai.Plate.Admin.Domain.OprationLog;

namespace ZhonTai.Plate.Admin.Service.OprationLog
{
    public interface IOprationLogService
    {
        Task<IResponseOutput> PageAsync(PageInput<OprationLogEntity> input);

        Task<IResponseOutput> AddAsync(OprationLogAddInput input);
    }
}