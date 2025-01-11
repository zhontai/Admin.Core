namespace MyApp.Api.Contracts.Services.Module.Output;

/// <summary>
/// 模块列表
/// </summary>
public class ModuleGetPageOutput
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
}