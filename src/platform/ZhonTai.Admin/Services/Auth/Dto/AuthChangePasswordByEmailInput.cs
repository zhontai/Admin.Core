using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Auth.Dto;

/// <summary>
/// 邮箱更改密码
/// </summary>
public class AuthChangePasswordByEmailInput
{
    /// <summary>
    /// 邮箱地址
    /// </summary>
    [Required(ErrorMessage = "请输入邮箱地址")]
    public string Email { get; set; }

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