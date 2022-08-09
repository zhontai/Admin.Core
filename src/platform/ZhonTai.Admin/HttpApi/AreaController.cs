using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Admin.HttpApi
{
    /// <summary>
    /// 域控制器
    /// </summary>
    [Area(AdminConsts.AreaName)]
    public abstract class AreaController : BaseController
    {
    }
}