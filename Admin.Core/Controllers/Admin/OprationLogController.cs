using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.OprationLog;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 操作日志管理
    /// </summary>
    public class OprationLogController : AreaController
    {
        private readonly IOprationLogService _oprationLogService;

        public OprationLogController(IOprationLogService loginLogService)
        {
            _oprationLogService = loginLogService;
        }

        /// <summary>
        /// 查询分页操作日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<OprationLogEntity> model)
        {
            return await _oprationLogService.PageAsync(model);
        }
    }
}