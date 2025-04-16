namespace ZhonTai.Admin.Services.Permission.Dto;

/// <summary>
/// 查询列表
/// </summary>
public class PermissionGetListInput
{
    /// <summary>
    /// 平台
    /// </summary>
    public string Platform { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    public string Label { get; set; }
}