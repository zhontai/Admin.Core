namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 用户
/// </summary>
public class UserGetOutput : UserUpdateInput
{
    /// <summary>
    /// 角色列表
    /// </summary>
    public ICollection<UserGetRoleModel> Roles { get; set; }

    /// <summary>
    /// 角色Ids
    /// </summary>
    public override long[] RoleIds => Roles?.Select(a => a.Id)?.ToArray();
}