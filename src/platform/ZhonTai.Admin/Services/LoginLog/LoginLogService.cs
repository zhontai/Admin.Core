using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.LoginLog;
using ZhonTai.Admin.Services.LoginLog.Dto;
using ZhonTai.Admin.Domain;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace ZhonTai.Admin.Services.LoginLog
{
    /// <summary>
    /// 登录日志服务
    /// </summary>
    [DynamicApi(Area = "admin")]
    public class LoginLogService : BaseService, ILoginLogService, IDynamicApi
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

        /// <summary>
        /// 查询登录日志列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> GetPageAsync(PageInput<LogGetPageDto> input)
        {
            var userName = input.Filter?.CreatedUserName;

            var list = await _loginLogRepository.Select
            .WhereDynamicFilter(input.DynamicFilter)
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

            return ResultOutput.Ok(data);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput<long>> AddAsync(LoginLogAddInput input)
        {
            var res = new ResultOutput<long>();

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