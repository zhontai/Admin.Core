namespace ZhonTai.Admin.Services.SearchTemplate.Inputs;

/// <summary>
/// 保存请求
/// </summary>
public class SearchTemplateSaveInput
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

    /// <summary>
    /// 版本
    /// </summary>
    public long Version { get; set; }
}