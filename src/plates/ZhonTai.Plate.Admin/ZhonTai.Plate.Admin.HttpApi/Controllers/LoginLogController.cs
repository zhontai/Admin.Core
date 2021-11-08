using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Plate.Admin.Service.LoginLog;

namespace ZhonTai.Plate.Admin.HttpApi
{
    /// <summary>
    /// 登录日志管理
    /// </summary>
    public class LoginLogController : AreaController
    {
        private readonly ILoginLogService _loginLogService;

        public LoginLogController(ILoginLogService loginLogService)
        {
            _loginLogService = loginLogService;
        }

        /// <summary>
        /// 查询分页登录日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> GetPage(PageInput<LogGetPageDto> input)
        {
            return await _loginLogService.GetPageAsync(input);
        }
    }
}