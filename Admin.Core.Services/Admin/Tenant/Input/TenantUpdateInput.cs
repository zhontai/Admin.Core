namespace Admin.Core.Service.Admin.Tenant.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class TenantUpdateInput : TenantAddInput
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
