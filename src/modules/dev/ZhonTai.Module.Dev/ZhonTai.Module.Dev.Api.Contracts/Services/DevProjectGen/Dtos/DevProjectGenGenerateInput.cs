using System.Collections.Generic;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos;

public class DevProjectGenGenerateInput
{
    /// <summary>
    /// 项目Id
    /// </summary>
    public long ProjectId { get; set; }

    /// <summary>
    /// 模型Ids 
    /// </summary>
    public List<long> ModelIds { get; set; } = new List<long>();

    /// <summary>
    /// 分组Ids
    /// </summary>
    public List<long> GroupIds { get; set; } = new List<long>();
    /// <summary>
    /// 模板Ids
    /// </summary>
    public List<long> TemplateIds { get; set; } = new List<long>();

    /// <summary>
    /// 是否是预览
    /// </summary>
    public bool IsPreview { get; set; }
}
