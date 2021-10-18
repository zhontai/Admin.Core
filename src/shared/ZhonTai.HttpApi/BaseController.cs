using Microsoft.AspNetCore.Mvc;
using ZhonTai.HttpApi.Attributes;

namespace ZhonTai.HttpApi
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