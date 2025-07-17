using System;
using System.Collections.Generic;
using System.Text;

namespace ZhonTai.Plugin.Lazy.SlideCaptcha.Core.Generator
{
    public interface ICaptchaImageGenerator
    {
        CaptchaImageData Generate();
    }
}
