namespace ZhonTai.Plate.Admin.Service.Dictionary.Dto
{
    /// <summary>
    /// 修改
    /// </summary>
    public class DictionaryUpdateInput : DictionaryAddInput
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