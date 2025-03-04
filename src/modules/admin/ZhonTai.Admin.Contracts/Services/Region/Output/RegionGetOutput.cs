namespace ZhonTai.Admin.Services.Region;

/// <summary>
/// 地区
/// </summary>
public class RegionGetOutput : RegionUpdateInput
{
    /// <summary>
    /// 上级Id列表
    /// </summary>
    public List<long> ParentIdList { get; set; }
}