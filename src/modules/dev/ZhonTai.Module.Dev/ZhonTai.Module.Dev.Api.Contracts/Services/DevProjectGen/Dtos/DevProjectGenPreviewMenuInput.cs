using System.Collections.Generic;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos;

/// <summary>
/// 项目生成预览
/// </summary>
public class DevProjectGenPreviewMenuInput
{
    /// <summary>
    /// 项目Id
    /// </summary>
    public long ProjectId { get; set; }

    /// <summary>
    /// 模板组
    /// </summary>
    public List<long> GroupIds { get; set; }

    /// <summary>
    /// 模板状态
    /// </summary>
    public bool? TemplateStatus { get; set; }
}
