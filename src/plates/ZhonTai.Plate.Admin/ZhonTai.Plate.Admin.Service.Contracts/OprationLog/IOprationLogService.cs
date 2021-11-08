using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Service.OprationLog.Input;
using ZhonTai.Plate.Admin.Domain;

namespace ZhonTai.Plate.Admin.Service.OprationLog
{
    public interface IOprationLogService
    {
        Task<IResultOutput> GetPageAsync(PageInput<LogGetPageDto> input);

        Task<IResultOutput> AddAsync(OprationLogAddInput input);
    }
}