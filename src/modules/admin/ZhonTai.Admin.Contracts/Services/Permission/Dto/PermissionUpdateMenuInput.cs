using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.Permission.Dto;

/// <summary>
/// 修改权限菜单
/// </summary>
public class PermissionUpdateMenuInput : PermissionAddMenuInput
{
    /// <summary>
    /// 权限Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择菜单")]
    public long Id { get; set; }
}