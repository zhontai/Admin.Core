namespace Admin.Core.Service.Personnel.Organization.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public class OrganizationUpdateInput : OrganizationAddInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}