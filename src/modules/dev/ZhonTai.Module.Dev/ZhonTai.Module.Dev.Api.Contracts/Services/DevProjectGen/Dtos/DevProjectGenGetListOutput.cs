using System;
using System.Collections.Generic;
using System.Linq;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos;

/// <summary>项目生成列表查询结果输出</summary>
public partial class DevProjectGenGetListOutput
{
    public long Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public string CreatedUserName { get; set; }
    public string ModifiedUserName { get; set; }
    public DateTime? ModifiedTime { get; set; }
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