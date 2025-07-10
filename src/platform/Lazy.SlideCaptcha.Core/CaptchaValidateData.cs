using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core
{
    public class CaptchaValidateData
    {
        public CaptchaValidateData(float percent, float tolerant)
        {
            Percent = percent;
            Tolerant = tolerant;
        }

        public float Percent { get; set; }
        public float Tolerant { get; set; }
    }
}
