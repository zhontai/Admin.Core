using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core.Validator
{
    public abstract class BaseValidator : IValidator
    {
        public bool Validate(SlideTrack slideTrack, CaptchaValidateData captchaValidateData)
        {
            if (slideTrack == null) throw new ArgumentNullException(nameof(slideTrack));
            if (captchaValidateData == null) throw new ArgumentNullException(nameof(captchaValidateData));

            slideTrack.Check();

            var min = captchaValidateData.Percent - captchaValidateData.Tolerant;
            var max = captchaValidateData.Percent + captchaValidateData.Tolerant;

            if (slideTrack.Percent < min || slideTrack.Percent > max)
            {
                return false;
            }

            return ValidateCore(slideTrack, captchaValidateData);
        }

        public abstract bool ValidateCore(SlideTrack slideTrack, CaptchaValidateData captchaValidateData);
    }
}
