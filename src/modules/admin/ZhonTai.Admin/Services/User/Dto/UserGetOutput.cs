using System.Collections.Generic;
using System.Linq;

namespace ZhonTai.Admin.Services.User.Dto;

public class UserGetOutput : UserUpdateInput
{
    /// <summary>
    /// 角色列表
    /// </summary>
    public ICollection<UserGetRoleDto> Roles { get; set; }

    /// <summary>
    /// 角色Ids
    /// </summary>
    public override long[] RoleIds => Roles?.Select(a => a.Id)?.ToArray();
}