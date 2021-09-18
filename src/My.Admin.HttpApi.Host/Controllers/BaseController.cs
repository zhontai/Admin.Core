using My.Admin.HttpApi.Host.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace My.Admin.HttpApi.Host.Controllers
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