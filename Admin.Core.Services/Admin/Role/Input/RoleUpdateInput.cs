namespace Admin.Core.Service.Admin.Role.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class RoleUpdateInput :RoleAddInput
    {
        /// <summary>
        /// 接口Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}
