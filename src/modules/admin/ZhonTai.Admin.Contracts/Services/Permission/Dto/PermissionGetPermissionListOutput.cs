namespace ZhonTai.Admin.Services.Permission.Dto;

/// <summary>
/// 权限列表
/// </summary>
public class PermissionGetPermissionListOutput
{
    /// <summary>
    /// 权限Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 父级节点
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 行显示
    /// </summary>
    public bool Row { get; set; }

    /// <summary>
    /// 权限列表
    /// </summary>
    public List<PermissionGetPermissionListOutput> Children { get; set; } = new List<PermissionGetPermissionListOutput>();
}