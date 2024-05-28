namespace MyApp.Api.Domain.Module.Output;

/// <summary>
/// 模块数据导出
/// </summary>
public partial class ModuleDataOutput
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
}