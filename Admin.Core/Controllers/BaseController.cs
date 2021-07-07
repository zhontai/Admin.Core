using Admin.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Core.Controllers
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