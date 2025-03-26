namespace ZhonTai.Admin.Services.PrintTemplate.Ouputs;

/// <summary>
/// 分页响应
/// </summary>
public class PrintTemplateGetPageOutput
{
    /// <summary>
    /// 打印模板Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
	public int Sort { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public long Version { get; set; }
}