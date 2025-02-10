namespace ZhonTai.Admin.Services.Role.Dto;

/// <summary>
/// 角色用户列表请求
/// </summary>
public partial class RoleGetRoleUserListInput
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
    public long? RoleId { get; set; }
}