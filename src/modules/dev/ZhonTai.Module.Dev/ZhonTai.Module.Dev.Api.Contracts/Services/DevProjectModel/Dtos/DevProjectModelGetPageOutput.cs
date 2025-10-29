using System;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectModel.Dtos;

/// <summary>项目模型分页查询结果输出</summary>
public partial class DevProjectModelGetPageOutput
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
    /// <summary>模型名称</summary>
    public string Name { get; set; }
    /// <summary>模型编码</summary>
    public string Code { get; set; }
    /// <summary>是否启用</summary>
    public bool IsEnable { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }
}