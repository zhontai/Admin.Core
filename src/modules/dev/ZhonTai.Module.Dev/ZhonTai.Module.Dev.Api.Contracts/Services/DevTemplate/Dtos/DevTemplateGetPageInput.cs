namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevTemplate.Dtos;

/// <summary>模板分页查询条件输入</summary>
public partial class DevTemplateGetPageInput
{
    /// <summary>模板名称</summary>       
    public string? Name { get; set; }
    /// <summary>模板分组</summary>       
    public long? GroupId { get; set; }
}