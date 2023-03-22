using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Core.Consts;
using Lazy.SlideCaptcha.Core;
using Lazy.SlideCaptcha.Core.Validator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Services.Cache;

/// <summary>
/// 验证码服务
/// </summary>
[Order(210)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class CaptchaService : BaseService, IDynamicApi
{
    private readonly ICaptcha _captcha;
    public CaptchaService(ICaptcha captcha)
    {
        _captcha = captcha;
    }

    /// <summary>
    /// 生成
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOprationLog]
    public CaptchaData Generate()
    {
        return _captcha.Generate();
    }

    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="id"></param>
    /// <param name="track"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOprationLog]
    public ValidateResult CheckAsync([FromQuery] string id, SlideTrack track)
    {
        return _captcha.Validate(id, track);
    }
}