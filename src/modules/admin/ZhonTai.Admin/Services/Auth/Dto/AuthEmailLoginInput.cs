using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Auth.Dto;

/// <summary>
/// 邮箱登录信息
/// </summary>
public class AuthEmailLoginInput
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
}