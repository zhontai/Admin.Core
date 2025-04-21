using Newtonsoft.Json;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.UserStaff;

namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 已删除用户分页查询响应
/// </summary>
public class UserGetDeletedUserPageOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; }

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

    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType Type { get; set; }

    [JsonIgnore]
    public ICollection<RoleEntity> Roles { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    public string RoleNames => string.Join("，", Roles?.Select(a => a.Name)?.ToArray());

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public Sex? Sex { get; set; }

    /// <summary>
    /// 主属部门Id
    /// </summary>
    [JsonIgnore]
    public long OrgId { get; set; }

    /// <summary>
    /// 主属部门
    /// </summary>
    public string OrgPath { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    [JsonIgnore]
    public ICollection<OrgEntity> Orgs { get; set; }

    /// <summary>
    /// 所属部门Id列表
    /// </summary>
    [JsonIgnore]
    public long[] OrgIds => Orgs?.Select(a => a.Id)?.ToArray();

    /// <summary>
    /// 所属部门
    /// </summary>
    public string OrgPaths { get; set; }

    /// <summary>
    /// 创建者用户名
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建者姓名
    /// </summary>
    public string CreatedUserRealName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 修改者用户名
    /// </summary>
    public string ModifiedUserName { get; set; }

    /// <summary>
    /// 修改者姓名
    /// </summary>
    public string ModifiedUserRealName { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModifiedTime { get; set; }
}