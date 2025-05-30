namespace ZhonTai.Admin.Services.SearchTemplate.Inputs;

/// <summary>
/// 添加请求
/// </summary>
public class SearchTemplateAddInput
{
    /// <summary>
    /// 模块Id
    /// </summary>
    public long ModuleId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 模板
    /// </summary>
    public string Template { get; set; }
}