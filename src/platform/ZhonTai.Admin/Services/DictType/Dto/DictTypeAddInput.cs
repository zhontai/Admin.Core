using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.DictType.Dto;

/// <summary>
/// 添加字典类型
/// </summary>
public class DictTypeAddInput
{
    /// <summary>
    /// 字典类型名称
    /// </summary>
    [Required(ErrorMessage = "请输入字典类型名称")]
    public string Name { get; set; }

    /// <summary>
    /// 字典类型编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}