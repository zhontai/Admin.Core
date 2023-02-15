using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Domain.User;

namespace ZhonTai.Admin.Services.User.Dto;

/// <summary>
/// 用户表单
/// </summary>
public class UserFormInput
{
    /// <summary>
    /// 账号
    /// </summary>
    [Required(ErrorMessage = "请输入账号")]
    public string UserName { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Required(ErrorMessage = "请输入姓名")]
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
    /// 角色Ids
    /// </summary>
    public virtual long[] RoleIds { get; set; }

    /// <summary>
    /// 所属部门Ids
    /// </summary>
    public virtual long[] OrgIds { get; set; }

    /// <summary>
    /// 主属部门Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// 直属主管Id
    /// </summary>
    public long? ManagerUserId { get; set; }

    /// <summary>
    /// 直属主管姓名
    /// </summary>
    public string ManagerUserName { get; set; }

    /// <summary>
    /// 员工
    /// </summary>
    public StaffAddInput Staff { get; set; }
}