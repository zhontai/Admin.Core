using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Core.Consts;
using Lazy.SlideCaptcha.Core;
using Lazy.SlideCaptcha.Core.Validator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Captcha;

namespace ZhonTai.Admin.Services.Cache;

/// <summary>
/// 验证码服务
/// </summary>
[Order(210)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class CaptchaService : BaseService, IDynamicApi
{
    private ICaptcha _captcha => LazyGetRequiredService<ICaptcha>();
    private ISlideCaptcha _slideCaptcha => LazyGetRequiredService<ISlideCaptcha>();
    public CaptchaService()
    {
    }

    /// <summary>
    /// 生成
    /// </summary>
    /// <param name="captchaId">验证码id</param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOprationLog]
    public CaptchaData Generate(string captchaId = null)
    {
        return _captcha.Generate(captchaId);
    }

    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="captchaId">验证码id</param>
    /// <param name="track">滑动轨迹</param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOprationLog]
    public ValidateResult CheckAsync([FromQuery] string captchaId, SlideTrack track)
    {
        return _slideCaptcha.Validate(captchaId, track, false);
    }
}