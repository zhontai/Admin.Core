namespace ZhonTai.Admin.Services.Auth.Dto;

/// <summary>
/// 用户权限
/// </summary>
public class AuthGetUserPermissionsOutput
{
    /// <summary>
    /// 用户权限列表
    /// </summary>
    public List<string> Permissions { get; set; }
}