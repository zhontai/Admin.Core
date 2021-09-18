using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.LoginLog;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace My.Admin.HttpApi.Host.Controllers.Admin
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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<LoginLogEntity> model)
        {
            return await _loginLogService.PageAsync(model);
        }
    }
}