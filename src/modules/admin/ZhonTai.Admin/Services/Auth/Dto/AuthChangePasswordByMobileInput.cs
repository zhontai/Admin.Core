using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Auth.Dto;

/// <summary>
/// 手机更改密码
/// </summary>
public class AuthChangePasswordByMobileInput
{
    /// <summary>
    /// 手机号
    /// </summary>
    [Required(ErrorMessage = "请输入手机号")]
    public string Mobile { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    [Required(ErrorMessage = "请输入验证码")]
    public string Code { get; set; }

    /// <summary>
    /// 验证码Id
    /// </summary>
    [Required(ErrorMessage = "请获取验证码")]
    public string CodeId { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    [Required(ErrorMessage = "请输入新密码")]
    public string NewPassword { get; set; }

    /// <summary>
    /// 确认新密码
    /// </summary>
    public string ConfirmPassword { get; set; }
}