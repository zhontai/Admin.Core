using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.Permission.Dto;

/// <summary>
/// 修改权限分组
/// </summary>
public class PermissionUpdateGroupInput : PermissionAddGroupInput
{
    /// <summary>
    /// 权限Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择分组")]
    public long Id { get; set; }
}