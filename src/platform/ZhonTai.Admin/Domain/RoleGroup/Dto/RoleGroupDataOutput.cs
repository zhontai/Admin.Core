namespace ZhonTai.Admin.Domain.RoleGroup.Dto;

/// <summary>
/// 角色分组导出
/// </summary>
public partial class RoleGroupDataOutput
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// 数据Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
	public int Sort { get; set; }
}