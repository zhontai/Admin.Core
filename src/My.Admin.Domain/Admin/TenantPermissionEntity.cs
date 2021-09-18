using My.Admin.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace My.Admin.Domain.Admin
{
    /// <summary>
    /// 租户权限
    /// </summary>
	[Table(Name = "ad_tenant_permission")]
    [Index("idx_{tablename}_01", nameof(TenantId) + "," + nameof(PermissionId), true)]
    public class TenantPermissionEntity : EntityAdd
    {
        /// <summary>
        /// 租户Id
        /// </summary>
		public long TenantId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
		public long PermissionId { get; set; }

        /// <summary>
        /// 租户
        /// </summary>
        public TenantEntity Tenant { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public PermissionEntity Permission { get; set; }
    }
}