using Microsoft.AspNetCore.Mvc;
using ZhonTai.Plate.Admin.HttpApi.Shared.Attributes;

namespace ZhonTai.Plate.Admin.HttpApi.Shared
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    [ValidatePermission]
    [ValidateInput]
    public abstract class BaseController : ControllerBase
    {
    }
}