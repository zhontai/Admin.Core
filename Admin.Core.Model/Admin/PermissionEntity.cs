using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 权限
    /// </summary>
	[Table(Name = "ad_permission")]
    [Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Label), true)]
    public class PermissionEntity : EntityFull, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10, CanUpdate = false)]
        public long? TenantId { get; set; }

        /// <summary>
        /// 父级节点
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Label { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        [Column(StringLength = 550)]
        public string Code { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        [Column(MapType = typeof(int),CanUpdate = false)]
        public PermissionType Type { get; set; }

        /// <summary>
        /// 视图
        /// </summary>
        public long? ViewId { get; set; }
        public ViewEntity View { get; set; }

        /// <summary>
        /// 接口
        /// </summary>
        public long? ApiId { get; set; }
        public ApiEntity Api { get; set; }

        /// <summary>
        /// 菜单访问地址
        /// </summary>
        [Column(StringLength = 500)]
        public string Path { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Column(StringLength = 100)]
        public string Icon { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
		public bool Hidden { get; set; } = false;

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;

        /// <summary>
        /// 可关闭
        /// </summary>
        public bool? Closable { get; set; }

        /// <summary>
        /// 打开组
        /// </summary>
        public bool? Opened { get; set; }

        /// <summary>
        /// 打开新窗口
        /// </summary>
        public bool? NewWindow { get; set; }

        /// <summary>
        /// 链接外显
        /// </summary>
        public bool? External { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; } = 0;

        /// <summary>
        /// 描述
        /// </summary>
        [Column(StringLength = 100)]
        public string Description { get; set; }
    }
}
