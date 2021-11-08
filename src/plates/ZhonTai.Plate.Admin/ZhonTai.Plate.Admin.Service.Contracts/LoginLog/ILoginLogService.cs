using ZhonTai.Common.Domain.Dto;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.Service.LoginLog.Input;
using ZhonTai.Plate.Admin.Domain;

namespace ZhonTai.Plate.Admin.Service.LoginLog
{
    public interface ILoginLogService
    {
        Task<IResultOutput> GetPageAsync(PageInput<LogGetPageDto> input);

        Task<IResultOutput<long>> AddAsync(LoginLogAddInput input);
    }
}