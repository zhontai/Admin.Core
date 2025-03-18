namespace ZhonTai.Admin.Services.PrintTemplate.Inputs;

/// <summary>
/// 分页请求
/// </summary>
public partial class PrintTemplateGetPageInput
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }
}