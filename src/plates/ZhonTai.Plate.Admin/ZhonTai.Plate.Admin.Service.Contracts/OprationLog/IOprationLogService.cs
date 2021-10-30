using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Service.OprationLog.Input;
using ZhonTai.Plate.Admin.Domain.OprationLog;

namespace ZhonTai.Plate.Admin.Service.OprationLog
{
    public interface IOprationLogService
    {
        Task<IResultOutput> GetPageAsync(PageInput<OprationLogEntity> input);

        Task<IResultOutput> AddAsync(OprationLogAddInput input);
    }
}