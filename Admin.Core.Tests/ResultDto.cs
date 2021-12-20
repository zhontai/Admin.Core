namespace Admin.Core.Tests
{
    public class ResultDto<T>
    {
        /// <summary>
        /// 是否成功标记
        /// </summary>
        public bool Success => Code == 1;

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }
}