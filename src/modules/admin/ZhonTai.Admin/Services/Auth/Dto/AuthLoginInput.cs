using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Domain.User;

namespace ZhonTai.Admin.Services.Auth.Dto;

/// <summary>
/// 登录信息
/// </summary>
public class AuthLoginInput
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 账号类型
    /// </summary>
    public AccountType AccountType { get; set; } = AccountType.UserName;

    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "密码不能为空")]
    public string Password { get; set; }

    /// <summary>
    /// 密码键
    /// </summary>
    public string PasswordKey { get; set; }

    /// <summary>
    /// 验证码Id
    /// </summary>
    public string CaptchaId { get; set; }

    /// <summary>
    /// 验证码数据
    /// </summary>
    public string CaptchaData { get; set; }
}