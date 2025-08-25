namespace ZhonTai.Plugin.Lazy.SlideCaptcha.Core.Validator;

public interface IValidator
{
    bool Validate(SlideTrack slideTrack, CaptchaValidateData captchaValidateData);
}
