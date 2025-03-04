using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Permission.Dto;

/// <summary>
/// 权限分配
/// </summary>
public class PermissionAssignInput
{
    [Required(ErrorMessage = "角色不能为空")]
    public long RoleId { get; set; }

    [Required(ErrorMessage = "权限不能为空")]
    public List<long> PermissionIds { get; set; }
}