using Microsoft.AspNetCore.Mvc;
using Admin.Core.Attributes;

namespace Admin.Core.Controllers
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    [Permission]
    [ValidateInput]
    public abstract class BaseController : ControllerBase
    {
    }
}