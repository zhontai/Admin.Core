using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Contracts.Services.Sso.Dto;

/// <summary>
/// 修改应用输入
/// </summary>
public class SsoAppManageUpdateInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// 回调地址
    /// </summary>
    public string CallbackUrl { get; set; }

    /// <summary>
    /// 状态：1启用 0禁用
    /// </summary>
    public int Status { get; set; } = 1;
}
