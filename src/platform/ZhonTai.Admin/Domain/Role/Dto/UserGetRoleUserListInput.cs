namespace ZhonTai.Admin.Domain.Role.Dto;

public partial class UserGetRoleUserListInput
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