using System.ComponentModel;

namespace ZhonTai.Admin.Domain.Region;

/// <summary>
/// 地区级别
/// </summary>
public enum RegionLevel
{
    /// <summary>
    /// 省份
    /// </summary>
    Province = 1,

    /// <summary>
    /// 城市
    /// </summary>
    City = 2,

    /// <summary>
    /// 县/区
    /// </summary>
    County = 3,

    /// <summary>
    /// 镇/乡/街道
    /// </summary>
    [Description("镇/乡")]
    Town = 4,

    /// <summary>
    /// 村/村委会/社区/居委会
    /// </summary>
    [Description("村/社区")]
    Vilage = 5
}