namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos;

/// <summary>
/// 项目生成模板请求
/// </summary>
public class DevProjectGenTemplateInput
{
    /// <summary>
    /// 项目ID
    /// </summary>
    public long ProjectId { get; set; }

    /// <summary>
    /// 模板组
    /// </summary>
    public long GroupId { get; set; }
}
