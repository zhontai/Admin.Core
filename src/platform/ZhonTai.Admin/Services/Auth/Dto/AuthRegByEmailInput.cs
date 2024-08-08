using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Auth.Dto;

/// <summary>
/// 邮箱注册
/// </summary>
public class AuthRegByEmailInput
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
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "请输入密码")]
    public string Password { get; set; }

    /// <summary>
    /// 企业名称
    /// </summary>
    [Required(ErrorMessage = "请输入企业名称")]
    public string CorpName { get; set; }
}