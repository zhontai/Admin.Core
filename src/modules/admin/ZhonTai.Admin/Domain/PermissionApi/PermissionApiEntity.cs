using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Domain.Api;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Domain.PermissionApi;

/// <summary>
/// 权限接口
/// </summary>
[Table(Name = "ad_permission_api")]
[Index("idx_{tablename}_01", nameof(PermissionId) + "," + nameof(ApiId), true)]
public class PermissionApiEntity : EntityAdd
{
    /// <summary>
    /// 权限Id
    /// </summary>
	public long PermissionId { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    [NotGen]
    public PermissionEntity Permission { get; set; }

    /// <summary>
    /// 接口Id
    /// </summary>
	public long ApiId { get; set; }

    /// <summary>
    /// 接口
    /// </summary>
    [NotGen]
    public ApiEntity Api { get; set; }
}