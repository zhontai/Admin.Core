namespace Admin.Core.Service.Admin.DictionaryType.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public class DictionaryTypeUpdateInput : DictionaryTypeAddInput
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