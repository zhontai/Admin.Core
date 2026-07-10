namespace ZhonTai.Admin.Services.Permission.Dto;

/// <summary>
/// 简单权限列表
/// </summary>
public class PermissionGetSimpleListOutput
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
}