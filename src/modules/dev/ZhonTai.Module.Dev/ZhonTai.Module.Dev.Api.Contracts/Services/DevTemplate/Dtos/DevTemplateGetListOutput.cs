using System;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevTemplate.Dtos;

/// <summary>模板列表查询结果输出</summary>
public partial class DevTemplateGetListOutput
{
    public long Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public string CreatedUserName { get; set; }
    public string ModifiedUserName { get; set; }
    public DateTime? ModifiedTime { get; set; }
    /// <summary>模板名称</summary>
    public string Name { get; set; }
    /// <summary>模板分组</summary>
    public long GroupId { get; set; }
    ///<summary>模板分组显示文本</summary>
    public string? GroupId_Text { get; set; }
    /// <summary>生成路径</summary>
    public string? OutTo { get; set; }
    /// <summary>是否启用</summary>
    public bool IsEnable { get; set; }
    /// <summary>模板内容</summary>
    public string Content { get; set; }
}

