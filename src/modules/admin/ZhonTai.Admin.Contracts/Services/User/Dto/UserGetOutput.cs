using System.Text.Json.Serialization;
using ZhonTai.Admin.Domain.Role;

namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 用户
/// </summary>
public class UserGetOutput : UserUpdateInput
{
    /// <summary>
    /// 角色列表
    /// </summary>
    [JsonIgnore]
    public ICollection<RoleEntity> Roles { get; set; }

    /// <summary>
    /// 角色Id列表
    /// </summary>
    public override long[] RoleIds => Roles?.Select(a => a.Id)?.ToArray();
}