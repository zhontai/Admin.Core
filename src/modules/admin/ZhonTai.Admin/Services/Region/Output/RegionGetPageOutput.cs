using ZhonTai.Admin.Domain.Region;

namespace ZhonTai.Admin.Services.Region;

public class RegionGetPageOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

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
    /// 拼音
    /// </summary>
    public string Pinyin { get; set; }

    /// <summary>
    /// 拼音首字母
    /// </summary>
    public string PinyinFirst { get; set; }

    /// <summary>
    /// 城乡分类代码
    /// </summary>
    public string VilageCode { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// 热门
    /// </summary>
    public bool Hot { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }
}