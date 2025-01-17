using System.Collections.Generic;

namespace ZhonTai.Admin.Services.Auth.Dto;

public class AuthGetUserInfoOutput
{
    /// <summary>
    /// 用户个人信息
    /// </summary>
    public AuthUserProfileDto User { get; set; }

    /// <summary>
    /// 用户菜单列表
    /// </summary>
    public List<AuthUserMenuDto> Menus { get; set; }

    /// <summary>
    /// 用户权限列表
    /// </summary>
    public List<string> Permissions { get; set; }
}