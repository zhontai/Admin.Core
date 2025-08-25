using ZhonTai.Plugin.Lazy.SlideCaptcha.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Captcha.Dto;

/// <summary>
/// 发送邮箱验证码
/// </summary>
public class SendEmailCodeInput
{
    /// <summary>
    /// 邮箱地址
    /// </summary>
    [Required(ErrorMessage = "请输入邮箱地址")]
    public string Email { get; set; }

    /// <summary>
    /// 验证码Id
    /// </summary>
    public string? CodeId { get; set; }

    /// <summary>
    /// 验证码Id
    /// </summary>
    [Required(ErrorMessage = "请完成安全验证")]
    public string CaptchaId { get; set; }

    /// <summary>
    /// 滑动轨迹
    /// </summary>
    [Required(ErrorMessage = "请完成安全验证")]
    public SlideTrack Track { get; set; }
}