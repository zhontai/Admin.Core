using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.Tenant.Dto;

/// <summary>
/// 修改
/// </summary>
public partial class TenantUpdateInput : TenantAddInput
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择租户")]
    public override long Id { get; set; }
}