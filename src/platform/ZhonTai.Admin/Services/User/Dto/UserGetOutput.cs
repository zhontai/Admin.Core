using System.Collections.Generic;

namespace ZhonTai.Admin.Services.User.Dto;

public class UserGetOutput : UserUpdateInput
{
    /// <summary>
    /// 角色列表
    /// </summary>
    public ICollection<UserGetRoleDto> Roles { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    public ICollection<UserGetOrgDto> Orgs { get; set; }
}