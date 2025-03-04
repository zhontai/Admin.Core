using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;

namespace ZhonTai.Admin.Domain.MsgType;

/// <summary>
/// 消息分类
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "msg_type")]
[Index("idx_{tablename}_01", $"{nameof(ParentId)},{nameof(Name)}", true)]
public partial class MsgTypeEntity : EntityBase
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 子级列表
    /// </summary>
    [Navigate(nameof(ParentId))]
    public List<MsgTypeEntity> Childs { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [Column(StringLength = 50)]
    public string Code { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
	public int Sort { get; set; }
}