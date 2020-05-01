namespace Admin.Core.Common.Output
{
    /// <summary>
    /// 下拉选项输出
    /// </summary>
    public class OptionOutput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 禁用
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 额外数据
        /// </summary>
        public object Data { get; set; }
    }
}
