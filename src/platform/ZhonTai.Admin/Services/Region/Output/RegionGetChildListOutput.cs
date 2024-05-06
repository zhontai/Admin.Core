using ZhonTai.Admin.Domain.Region;

namespace ZhonTai.Admin.Services.Region;

public class RegionGetChildListOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 级别
    /// </summary>
    public RegionLevel Level { get; set; }

    /// <summary>
    /// 拼音
    /// </summary>
    public string Pinyin { get; set; }

    /// <summary>
    /// 拼音首字母
    /// </summary>
    public string PinyinFirst { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 热门
    /// </summary>
    public bool Hot { get; set; }

    /// <summary>
    /// 叶子节点
    /// </summary>
    public bool Leaf { get => Level >= RegionLevel.Vilage; }
}