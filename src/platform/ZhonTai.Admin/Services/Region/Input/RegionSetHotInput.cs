namespace ZhonTai.Admin.Services.Region;

/// <summary>
/// 设置热门
/// </summary>
public class RegionSetHotInput
{
    /// <summary>
    /// 地区Id
    /// </summary>
    public long RegionId { get; set; }

    /// <summary>
    /// 热门
    /// </summary>
    public bool Hot { get; set; }
}