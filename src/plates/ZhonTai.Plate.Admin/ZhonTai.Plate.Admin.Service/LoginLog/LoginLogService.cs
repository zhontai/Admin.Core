using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ZhonTai.Common.Helpers;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain.LoginLog;
using ZhonTai.Plate.Admin.Service.LoginLog.Output;
using ZhonTai.Plate.Admin.Service.LoginLog.Input;

namespace ZhonTai.Plate.Admin.Service.LoginLog
{
    public class LoginLogService : BaseService, ILoginLogService
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILoginLogRepository _loginLogRepository;

        public LoginLogService(
            IHttpContextAccessor context,
            ILoginLogRepository loginLogRepository
        )
        {
            _context = context;
            _loginLogRepository = loginLogRepository;
        }

        public async Task<IResponseOutput> PageAsync(PageInput<LoginLogEntity> input)
        {
            var userName = input.Filter?.CreatedUserName;

            var list = await _loginLogRepository.Select
            .WhereIf(userName.NotNull(), a => a.CreatedUserName.Contains(userName))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<LoginLogListOutput>();

            var data = new PageOutput<LoginLogListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput<long>> AddAsync(LoginLogAddInput input)
        {
            var res = new ResponseOutput<long>();

            input.IP = IPHelper.GetIP(_context?.HttpContext?.Request);

            string ua = _context.HttpContext.Request.Headers["User-Agent"];
            if (ua.NotNull())
            {
                var client = UAParser.Parser.GetDefault().Parse(ua);
                var device = client.Device.Family;
                device = device.ToLower() == "other" ? "" : device;
                input.Browser = client.UA.Family;
                input.Os = client.OS.Family;
                input.Device = device;
                input.BrowserInfo = ua;
            }
            var entity = Mapper.Map<LoginLogEntity>(input);
            var id = (await _loginLogRepository.InsertAsync(entity)).Id;

            return id > 0 ? res.Ok(id) : res;
        }
    }
}