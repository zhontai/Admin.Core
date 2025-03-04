using Lazy.SlideCaptcha.Core;
using Lazy.SlideCaptcha.Core.Storage;
using Lazy.SlideCaptcha.Core.Validator;

namespace ZhonTai.Admin.Core.Captcha
{
    public class SlideCaptcha: ISlideCaptcha
    {
        private IValidator _validator;
        private IStorage _storage;

        public SlideCaptcha(IValidator validator, IStorage storage)
        {
            _storage = storage;
            _validator = validator;
        }

        public ValidateResult Validate(string captchaId, SlideTrack slideTrack, bool removeIfSuccess = true)
        {
             
            var captchaValidateData = _storage.Get<CaptchaValidateData>(captchaId);
            if (captchaValidateData == null) return ValidateResult.Timeout();
            var success = _validator.Validate(slideTrack, captchaValidateData);
            if (!success || (success && removeIfSuccess))
            {
                _storage.Remove(captchaId);
            }

            return success ? ValidateResult.Success() : ValidateResult.Fail();
            
        }
    }
}
