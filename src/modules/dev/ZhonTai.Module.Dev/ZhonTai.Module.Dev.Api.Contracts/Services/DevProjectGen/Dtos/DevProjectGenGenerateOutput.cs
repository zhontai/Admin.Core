namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos;

public class DevProjectGenGenerateOutput
{
    /// <summary>
    /// 模板id 
    /// </summary>
    public long TemplateId { get; set; }
    /// <summary>
    /// 输出路径
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 生成内容
    /// </summary>
    public string Content { get; set; }
}
