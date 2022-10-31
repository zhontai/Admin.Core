namespace ZhonTai.Admin.Services.DictionaryType.Dto;

/// <summary>
/// 添加
/// </summary>
public class DictionaryTypeAddInput
{
    /// <summary>
    /// 字典名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 字典编码
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
}