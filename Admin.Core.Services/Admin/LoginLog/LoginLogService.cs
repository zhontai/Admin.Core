using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.LoginLog.Input;
using Admin.Core.Service.Admin.LoginLog.Output;
using Admin.Core.Common.Helpers;

namespace Admin.Core.Service.Admin.LoginLog
{	
	public class LoginLogService : ILoginLogService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _context;
        private readonly ILoginLogRepository _loginLogRepository;
        public LoginLogService(
            IMapper mapper,
            IHttpContextAccessor context,
            ILoginLogRepository loginLogRepository
        )
        {
            _mapper = mapper;
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
            var entity = _mapper.Map<LoginLogEntity>(input);
            var id = (await _loginLogRepository.InsertAsync(entity)).Id;

            return id > 0 ? res.Ok(id) : res;
        }
    }
}
