namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevGroup.Dtos;

/// <summary>模板组查询结果输出</summary>
public partial class DevGroupGetOutput
{
    public long Id { get; set; }
    /// <summary>模板组名称</summary>
    public string Name { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }
}