using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.Dictionary.Dto;

/// <summary>
/// 添加字典
/// </summary>
public class DictionaryAddInput
{
    /// <summary>
    /// 字典类型Id
    /// </summary>
    [ValidateRequired(ErrorMessage = "请选择字典类型")]
    public long DictionaryTypeId { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    [Required(ErrorMessage = "请输入字典名称")]
    public string Name { get; set; }

    /// <summary>
    /// 字典编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 字典值
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; }
}