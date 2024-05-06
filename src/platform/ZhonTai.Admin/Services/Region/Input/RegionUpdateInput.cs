using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.Region;


/// <summary>
/// 修改
/// </summary>
public class RegionUpdateInput : RegionAddInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择地区")]
    public long Id { get; set; }
}