namespace ZhonTai.Admin.Services.DictionaryType.Dto;

public class DictionaryTypeListOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 字典编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; }
}