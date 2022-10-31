using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Core;

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