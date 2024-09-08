using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.Api.Dto;

/// <summary>
/// 修改
/// </summary>
public partial class ApiUpdateInput : ApiAddInput
{
    /// <summary>
    /// 接口Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择接口")]
    public long Id { get; set; }
}