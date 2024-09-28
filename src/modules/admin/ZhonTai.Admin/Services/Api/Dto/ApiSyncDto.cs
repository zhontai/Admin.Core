namespace ZhonTai.Admin.Services.Api.Dto;

/// <summary>
/// 接口同步Dto
/// </summary>
public class ApiSyncDto
{
    /// <summary>
    /// 接口名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 父级路径
    /// </summary>
    public string ParentPath { get; set; }

    /// <summary>
    /// 接口提交方法
    /// </summary>
    public string HttpMethods { get; set; }
}