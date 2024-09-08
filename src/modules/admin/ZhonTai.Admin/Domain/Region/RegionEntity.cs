using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ZhonTai.Admin.Domain.Region;

/// <summary>
/// 地区
/// </summary>
[Table(Name = "base_region")]
[Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Name), true)]
[Index("idx_{tablename}_02", nameof(ParentId) + "," + nameof(Code), true)]
public partial class RegionEntity : EntityBase
{
    /// <summary>
    /// 上级Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Column(StringLength = 100)]
    public string Name { get; set; }

    /// <summary>
    /// 级别
    /// </summary>
    [Column(MapType = typeof(int))]
    public RegionLevel Level { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    [Column(StringLength = 20)]
    public string Code { get; set; }

    /// <summary>
    /// 拼音
    /// </summary>
    [Column(StringLength = 200)]
    public string Pinyin { get; set; }

    /// <summary>
    /// 拼音首字母
    /// </summary>
    [Column(StringLength = 20)]
    public string PinyinFirst { get; set; }

    /// <summary>
    /// 提取地址
    /// </summary>
    [Column(StringLength = 100)]
    public string Url { get; set; }

    /// <summary>
    /// 城乡分类代码
    /// </summary>
    [Column(StringLength = 10)]
    public string VilageCode { get; set; }

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

    [Navigate(nameof(ParentId))]
    public List<RegionEntity> Childs { get; set; }
}