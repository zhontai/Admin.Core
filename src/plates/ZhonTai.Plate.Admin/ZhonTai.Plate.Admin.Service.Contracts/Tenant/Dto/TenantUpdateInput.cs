namespace ZhonTai.Plate.Admin.Service.Tenant.Dto
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class TenantUpdateInput : TenantAddInput
    {
        /// <summary>
        /// 接口Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}