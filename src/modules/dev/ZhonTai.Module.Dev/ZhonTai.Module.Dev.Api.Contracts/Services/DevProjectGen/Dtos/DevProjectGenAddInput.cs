using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos;

/// <summary>项目生成新增输入</summary>
public partial class DevProjectGenAddInput
{
    /// <summary>所属项目</summary>
    [Required(ErrorMessage = "所属项目不能为空")]
    public long ProjectId { get; set; }
    /// <summary>模板组</summary>
    [Required(ErrorMessage = "模板组不能为空")]
    public string GroupIds { get { return string.Join(',', GroupIds_Values ?? new List<string>()); } }
    ///<summary>页面提交的模板组数组</summary>
    public List<string>? GroupIds_Values { get; set; }
}