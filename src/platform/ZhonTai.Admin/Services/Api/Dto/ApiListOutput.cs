namespace ZhonTai.Admin.Services.Api.Dto;

public class ApiListOutput
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
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }
}