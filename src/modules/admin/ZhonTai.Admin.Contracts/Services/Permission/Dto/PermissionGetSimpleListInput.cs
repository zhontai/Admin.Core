using ZhonTai.Admin.Domain.Permission;

namespace ZhonTai.Admin.Services.Permission.Dto;

/// <summary>
/// 查询简单权限列表
/// </summary>
public class PermissionGetSimpleListInput
{
    /// <summary>
    /// 平台
    /// </summary>
    public string Platform { get; set; }

    /// <summary>
    /// 权限类型
    /// </summary>
    public PermissionType Type { get; set; }
}