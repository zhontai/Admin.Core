namespace ZhonTai.Admin.Services.Api.Dto;

public class ApiGetListOutput
{
    /// <summary>
    /// 接口Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 接口父级
    /// </summary>
	public long? ParentId { get; set; }

    /// <summary>
    /// 接口命名
    /// </summary>
    public string Name { get; set; }

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
    /// 启用请求参数
    /// </summary>
    public bool EnabledParams { get; set; }

    /// <summary>
    /// 启用响应结果
    /// </summary>
    public bool EnabledResult { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }
}