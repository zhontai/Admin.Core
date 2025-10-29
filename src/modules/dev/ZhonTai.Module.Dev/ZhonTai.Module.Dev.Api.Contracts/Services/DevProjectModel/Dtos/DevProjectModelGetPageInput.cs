namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectModel.Dtos;

/// <summary>项目模型分页查询条件输入</summary>
public partial class DevProjectModelGetPageInput
{
    /// <summary>所属项目</summary>       
    public long? ProjectId { get; set; }
    /// <summary>模型名称</summary>       
    public string? Name { get; set; }
    /// <summary>模型编码</summary>       
    public string? Code { get; set; }
}