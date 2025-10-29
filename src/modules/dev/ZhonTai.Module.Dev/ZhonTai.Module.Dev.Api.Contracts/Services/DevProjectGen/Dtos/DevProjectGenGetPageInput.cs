namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos;

/// <summary>项目生成分页查询条件输入</summary>
public partial class DevProjectGenGetPageInput
{
    /// <summary>所属项目</summary>       
    public long? ProjectId { get; set; }
}