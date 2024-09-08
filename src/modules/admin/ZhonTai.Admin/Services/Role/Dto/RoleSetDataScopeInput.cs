using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;
using ZhonTai.Admin.Domain.Role;

namespace ZhonTai.Admin.Services.Role.Dto;

/// <summary>
/// 设置数据范围
/// </summary>
public class RoleSetDataScopeInput
{
    /// <summary>
    /// 角色Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择角色")]
    public long RoleId { get; set; }

    /// <summary>
    /// 数据范围
    /// </summary>
    public DataScope DataScope { get; set; }

    /// <summary>
    /// 指定部门
    /// </summary>
    public long[] OrgIds { get; set; }
}