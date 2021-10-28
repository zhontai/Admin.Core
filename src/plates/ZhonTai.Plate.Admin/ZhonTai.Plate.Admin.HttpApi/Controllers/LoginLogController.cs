using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain.LoginLog;
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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<LoginLogEntity> model)
        {
            return await _loginLogService.PageAsync(model);
        }
    }
}