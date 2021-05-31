namespace Admin.Core.Service.Admin.View.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public class ViewUpdateInput : ViewAddInput
    {
        /// <summary>
        /// 视图Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}
