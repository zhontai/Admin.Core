namespace ZhonTai.Admin.Services.Region;

/// <summary>
/// 地区
/// </summary>
public class RegionGetOutput : RegionUpdateInput
{
    public List<long> ParentIdList { get; set; }
}