namespace Admin.Core.Service.Admin.Role.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class RoleAddInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; }
    }
}
