namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectModel.Dtos;

/// <summary>项目模型查询结果输出</summary>
public partial class DevProjectModelGetOutput
{
    public long Id { get; set; }
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