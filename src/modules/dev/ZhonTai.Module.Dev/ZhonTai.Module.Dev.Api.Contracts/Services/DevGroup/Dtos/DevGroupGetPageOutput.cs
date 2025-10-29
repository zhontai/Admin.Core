using System;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevGroup.Dtos;

/// <summary>模板组分页查询结果输出</summary>
public partial class DevGroupGetPageOutput
{
    public long Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public string CreatedUserName { get; set; }
    public string ModifiedUserName { get; set; }
    public DateTime? ModifiedTime { get; set; }
    /// <summary>模板组名称</summary>
    public string Name { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }
}