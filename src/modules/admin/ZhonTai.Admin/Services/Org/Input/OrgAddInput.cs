namespace ZhonTai.Admin.Services.Org.Input;

/// <summary>
/// 添加
/// </summary>
public class OrgAddInput
{
    /// <summary>
    /// 父级
    /// </summary>
	public long ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
	public int Sort { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
}