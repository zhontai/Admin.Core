using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.Tenant.Dto;

/// <summary>
/// 修改
/// </summary>
public partial class TenantUpdateInput : TenantAddInput
{
    /// <summary>
    /// 接口Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择租户")]
    public long Id { get; set; }
}