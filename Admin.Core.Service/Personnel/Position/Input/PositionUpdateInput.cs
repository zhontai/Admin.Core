namespace Admin.Core.Service.Personnel.Position.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class PositionUpdateInput : PositionAddInput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}