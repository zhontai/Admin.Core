using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZhonTai.Common.Input;
using ZhonTai.Common.Output;
using ZhonTai.Plate.Admin.Domain.OprationLog;
using ZhonTai.Plate.Admin.Service.OprationLog;

namespace ZhonTai.Plate.Admin.HttpApi
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