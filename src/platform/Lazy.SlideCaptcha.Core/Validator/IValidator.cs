
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core.Validator
{
    public interface IValidator
    {
        bool Validate(SlideTrack slideTrack, CaptchaValidateData captchaValidateData);
    }
}
