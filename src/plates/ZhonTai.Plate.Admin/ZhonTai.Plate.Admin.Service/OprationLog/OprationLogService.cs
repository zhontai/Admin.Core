using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZhonTai.Common.Helpers;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Service.OprationLog.Dto;
using ZhonTai.Plate.Admin.Domain.OprationLog;
using ZhonTai.Plate.Admin.Domain;

namespace ZhonTai.Plate.Admin.Service.OprationLog
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

        public async Task<IResultOutput> GetPageAsync(PageInput<LogGetPageDto> input)
        {
            var userName = input.Filter?.CreatedUserName;

            var list = await _oprationLogRepository.Select
            .WhereDynamicFilter(input.DynamicFilter)
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

            return ResultOutput.Ok(data);
        }

        public async Task<IResultOutput> AddAsync(OprationLogAddInput input)
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

            return ResultOutput.Result(id > 0);
        }
    }
}