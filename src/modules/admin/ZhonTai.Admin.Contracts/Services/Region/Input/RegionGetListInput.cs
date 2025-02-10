namespace ZhonTai.Admin.Services.Region;

/// <summary>
/// 地区列表请求
/// </summary>
public class RegionGetListInput
{
    /// <summary>
    /// 上级Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 热门
    /// </summary>
    public bool? Hot { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool? Enabled { get; set; }
}