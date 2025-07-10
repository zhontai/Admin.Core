using Lazy.SlideCaptcha.Core.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core
{
    public interface ICaptcha
    {
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="captchaId">验证码id</param>
        /// <returns></returns>
        CaptchaData Generate(string captchaId = null);

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="captchaId">验证码id</param>
        /// <param name="slideTrack">滑动轨迹</param>
        /// <returns></returns>
        ValidateResult Validate(string captchaId, SlideTrack slideTrack);
    }
}
