using Admin.Core.Model.Admin;

namespace Admin.Core.Service.Admin.Permission.Input
{
    public class PermissionAddGroupInput
    {
        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// 父级节点
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Label { get; set; }

        ///// <summary>
        ///// 说明
        ///// </summary>
        //public string Description { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
		public bool Hidden { get; set; }

        ///// <summary>
        ///// 启用
        ///// </summary>
        //public bool Enabled { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 打开
        /// </summary>
        public bool? Opened { get; set; }
    }
}
