using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.Org.Input;

/// <summary>
/// 修改
/// </summary>
public class OrgUpdateInput : OrgAddInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择部门")]
    public long Id { get; set; }
}