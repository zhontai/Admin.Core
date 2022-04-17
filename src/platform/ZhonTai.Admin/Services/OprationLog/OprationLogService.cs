using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.OprationLog.Dto;
using ZhonTai.Admin.Domain.OprationLog;
using ZhonTai.Admin.Domain;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace ZhonTai.Admin.Services.OprationLog
{
    /// <summary>
    /// 操作日志服务
    /// </summary>
    [DynamicApi(Area = "admin")]
    public class OprationLogService : BaseService, IOprationLogService, IDynamicApi
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

        /// <summary>
        /// 查询操作日志列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
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

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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