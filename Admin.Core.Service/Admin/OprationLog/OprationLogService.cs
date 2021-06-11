using Admin.Core.Common.Helpers;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.OprationLog.Input;
using Admin.Core.Service.Admin.OprationLog.Output;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Admin.Core.Service.Admin.OprationLog
{
    public class OprationLogService : BaseService, IOprationLogService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IOprationLogRepository _oprationLogRepository;

        public OprationLogService(
            IHttpContextAccessor context,
            IOprationLogRepository oprationLogRepository
        )
        {
            _context = context;
            _oprationLogRepository = oprationLogRepository;
        }

        public async Task<IResponseOutput> PageAsync(PageInput<OprationLogEntity> input)
        {
            var userName = input.Filter?.CreatedUserName;

            var list = await _oprationLogRepository.Select
            .WhereIf(userName.NotNull(), a => a.CreatedUserName.Contains(userName))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<OprationLogListOutput>();

            var data = new PageOutput<OprationLogListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> AddAsync(OprationLogAddInput input)
        {
            string ua = _context.HttpContext.Request.Headers["User-Agent"];
            var client = UAParser.Parser.GetDefault().Parse(ua);
            var device = client.Device.Family;
            device = device.ToLower() == "other" ? "" : device;
            input.Browser = client.UA.Family;
            input.Os = client.OS.Family;
            input.Device = device;
            input.BrowserInfo = ua;

            input.NickName = User.NickName;
            input.IP = IPHelper.GetIP(_context?.HttpContext?.Request);

            var entity = Mapper.Map<OprationLogEntity>(input);
            var id = (await _oprationLogRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id > 0);
        }
    }
}