namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectModelField.Dtos;

/// <summary>项目模型字段分页查询条件输入</summary>
public partial class DevProjectModelFieldGetPageInput
{
    /// <summary>所属模型</summary>       
    public long? ModelId { get; set; }
    /// <summary>字段名称</summary>       
    public string? Name { get; set; }
}