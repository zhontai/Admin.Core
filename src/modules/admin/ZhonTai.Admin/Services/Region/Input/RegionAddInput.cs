using ZhonTai.Admin.Domain.Region;

namespace ZhonTai.Admin.Services.Region;

/// <summary>
/// 添加
/// </summary>
public class RegionAddInput
{
    /// <summary>
    /// 上级Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 级别
    /// </summary>
    public RegionLevel Level { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 提取Url
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// 热门
    /// </summary>
    public bool Hot { get; set; } = false;

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;
}