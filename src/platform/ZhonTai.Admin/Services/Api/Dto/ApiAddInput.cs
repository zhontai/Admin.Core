namespace ZhonTai.Admin.Services.Api.Dto;

/// <summary>
/// 添加
/// </summary>
public class ApiAddInput
{
    /// <summary>
    /// 所属模块
    /// </summary>
	public long? ParentId { get; set; }

    /// <summary>
    /// 接口名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 接口提交方法
    /// </summary>
    public string HttpMethods { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }
}