using System.Collections.Generic;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos;

public class DevProjectGenPreviewMenuOutput
{
    /// <summary>
    /// 分组ID
    /// </summary>
    public long GroupId { get; set; }

    /// <summary>
    /// 分组名
    /// </summary>
    public string GroupName { get; set; }

    /// <summary>
    /// 模板列表
    /// </summary>
    public List<DevProjectGenPreviewTemplateOutput> TemplateList { get; set; }
}