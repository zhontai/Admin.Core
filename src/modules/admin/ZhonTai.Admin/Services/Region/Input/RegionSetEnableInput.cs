namespace ZhonTai.Admin.Services.Region;

/// <summary>
/// 设置启用
/// </summary>
public class RegionSetEnableInput
{
    /// <summary>
    /// 地区Id
    /// </summary>
    public long RegionId { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
}