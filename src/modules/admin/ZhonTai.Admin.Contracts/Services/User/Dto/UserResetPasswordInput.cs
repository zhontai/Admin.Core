using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 重置密码
/// </summary>
public class UserResetPasswordInput : Entity
{
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}