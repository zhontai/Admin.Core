using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.Dictionary.Dto;

/// <summary>
/// 修改
/// </summary>
public class DictionaryUpdateInput : DictionaryAddInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择数据字典")]
    public long Id { get; set; }
}