using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Domain.DictType;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Domain.Dict;

/// <summary>
/// 数据字典
/// </summary>
[Table(Name = "ad_dict", OldName = "ad_dictionary")]
[Index("idx_{tablename}_01", nameof(DictTypeId) + "," + nameof(Name), true)]
public partial class DictEntity : EntityBase
{
    /// <summary>
    /// 字典类型Id
    /// </summary>
    [Column(OldName = "DictionaryTypeId")]
    public long DictTypeId { get; set; }

    /// <summary>
    /// 字典类型
    /// </summary>
    [NotGen]
    public DictTypeEntity DictType { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// 字典编码
    /// </summary>
    [Column(StringLength = 50)]
    public string Code { get; set; }

    /// <summary>
    /// 字典值
    /// </summary>
    [Column(StringLength = 50)]
    public string Value { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
	public int Sort { get; set; }
}