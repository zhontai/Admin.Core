using ZhonTai.Common.Domain.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Plate.Admin.Domain.Permission;
using ZhonTai.Plate.Admin.Domain.Api;

namespace ZhonTai.Plate.Admin.Domain.PermissionApi
{
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
        public PermissionEntity Permission { get; set; }

        /// <summary>
        /// 接口Id
        /// </summary>
		public long ApiId { get; set; }

        /// <summary>
        /// 接口
        /// </summary>
        public ApiEntity Api { get; set; }
    }
}