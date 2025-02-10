namespace ZhonTai.Admin.Services.Auth.Dto;

/// <summary>
/// 用户信息
/// </summary>
public class AuthGetUserInfoOutput
{
    /// <summary>
    /// 用户个人信息
    /// </summary>
    public AuthUserProfileOutput User { get; set; }

    /// <summary>
    /// 用户菜单列表
    /// </summary>
    public List<AuthUserMenuOutput> Menus { get; set; }

    /// <summary>
    /// 用户权限列表
    /// </summary>
    public List<string> Permissions { get; set; }
}