namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProject.Dtos;

/// <summary>项目分页查询条件输入</summary>
public partial class DevProjectGetPageInput
{
    /// <summary>项目名称</summary>       
    public string? Name { get; set; }
    /// <summary>项目编码</summary>       
    public string? Code { get; set; }
}