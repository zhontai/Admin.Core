using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Admin.Services.Pkg.Dto;

/// <summary>
/// 设置套餐权限
/// </summary>
public class PkgSetPkgPermissionsInput
{
    /// <summary>
    /// 平台
    /// </summary>
    public string Platform { get; set; } = AdminConsts.WebName;

    [Required(ErrorMessage = "套餐不能为空")]
    public long PkgId { get; set; }

    [Required(ErrorMessage = "权限不能为空")]
    public List<long> PermissionIds { get; set; }
}