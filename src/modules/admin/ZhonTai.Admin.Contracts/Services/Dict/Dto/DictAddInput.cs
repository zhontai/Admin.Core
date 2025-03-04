using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Validators;

namespace ZhonTai.Admin.Services.Dict.Dto;

/// <summary>
/// 添加字典
/// </summary>
public class DictAddInput
{
    /// <summary>
    /// 字典类型Id
    /// </summary>
    [ValidateRequired(ErrorMessage = "请选择字典类型")]
    public long DictTypeId { get; set; }

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
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? Sort { get; set; }
}