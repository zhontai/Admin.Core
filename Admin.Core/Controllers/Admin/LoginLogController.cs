using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.LoginLog;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.Core.Controllers.Admin
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