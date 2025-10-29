namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProject.Dtos;

/// <summary>项目查询结果输出</summary>
public partial class DevProjectGetOutput
{
    public long Id { get; set; }
    /// <summary>项目名称</summary>
    public string Name { get; set; }
    /// <summary>项目编码</summary>
    public string Code { get; set; }
    /// <summary>是否启用</summary>
    public bool IsEnable { get; set; }
    /// <summary>使用模板组</summary>
    public long GroupId { get; set; }
    ///<summary>使用模板组显示文本</summary>
    public string? GroupId_Text { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }
}