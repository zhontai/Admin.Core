using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 添加
/// </summary>
public class UserAddInput: UserFormInput
{
    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "请输入密码")]
    public virtual string Password { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;
}