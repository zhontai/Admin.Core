using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 会员表单
/// </summary>
public class UserMemberFormInput
{
    /// <summary>
    /// 账号
    /// </summary>
    [Required(ErrorMessage = "请输入账号")]
    public string UserName { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }
}