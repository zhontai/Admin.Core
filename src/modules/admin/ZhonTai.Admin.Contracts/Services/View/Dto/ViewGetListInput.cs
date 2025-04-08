namespace ZhonTai.Admin.Services.View.Dto;

/// <summary>
/// 视图查询
/// </summary>
public class ViewGetListInput
{
    /// <summary>
    /// 视图分类
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 视图命名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 视图名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 视图路径
    /// </summary>
    public string Path { get; set; }
}