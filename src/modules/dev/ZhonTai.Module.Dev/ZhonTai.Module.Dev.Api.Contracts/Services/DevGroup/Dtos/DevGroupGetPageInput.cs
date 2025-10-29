namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevGroup.Dtos;

/// <summary>模板组分页查询条件输入</summary>
public partial class DevGroupGetPageInput
{
    /// <summary>模板组名称</summary>       
    public string? Name { get; set; }
    /// <summary>
    /// 模板Id
    /// </summary>
    public long? Id { get; set; }
}