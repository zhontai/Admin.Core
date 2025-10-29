namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos;

public class DevProjectGenPreviewTemplateOutput
{
    /// <summary>
    /// 模板组Id
    /// </summary>
    public long GroupId { get; set; }
    /// <summary>
    /// 模板Id
    /// </summary>
    public long TemplateId { get; set; }

    /// <summary>
    /// 模板名称
    /// </summary>
    public string TemplateName { get; set; }

    /// <summary>
    /// 模板生成路径
    /// </summary>
    public string TempaltePath { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnable { get; set; }
}