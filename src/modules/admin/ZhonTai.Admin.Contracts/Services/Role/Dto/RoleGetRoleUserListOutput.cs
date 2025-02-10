namespace ZhonTai.Admin.Services.Role.Dto;

/// <summary>
/// 角色用户列表响应
/// </summary>
public class RoleGetRoleUserListOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

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