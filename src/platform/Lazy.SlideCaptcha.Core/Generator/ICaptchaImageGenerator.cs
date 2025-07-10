using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core.Generator
{
    public interface ICaptchaImageGenerator
    {
        CaptchaImageData Generate();
    }
}
