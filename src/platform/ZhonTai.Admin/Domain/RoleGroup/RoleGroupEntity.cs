using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;

namespace ZhonTai.Admin.Domain.RoleGroup;

/// <summary>
/// 角色分组
/// </summary>
[Table(Name = "ad_role_group")]
[Index("idx_{tablename}_01", nameof(TenantId) + "," + nameof(Name), true)]
public class RoleGroupEntity : EntityFull, ITenant
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [Column(Position = 2, CanUpdate = false)]
    public long? TenantId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
	public int Sort { get; set; }
}