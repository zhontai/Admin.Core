using System;
using System.Collections.Generic;
using System.Linq;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos;

/// <summary>项目生成查询结果输出</summary>
public partial class DevProjectGenGetOutput
{
    public long Id { get; set; }
    /// <summary>所属项目</summary>
    public long ProjectId { get; set; }
    ///<summary>所属项目显示文本</summary>
    public string? ProjectId_Text { get; set; }
    /// <summary>模板组</summary>
    public string GroupIds { get; set; }
    ///<summary>模板组显示文本</summary>
    public List<string>? GroupIds_Texts { get; set; }
    ///<summary>页面使用的模板组数组</summary>
    public List<string>? GroupIds_Values { get { return GroupIds?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(); } }
}