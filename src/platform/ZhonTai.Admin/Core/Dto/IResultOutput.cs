using System.Text.Json.Serialization;

namespace ZhonTai.Admin.Core.Dto
{
    /// <summary>
    /// 结果输出接口
    /// </summary>
    public interface IResultOutput
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonIgnore]
        bool Success { get; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; }
    }

    /// <summary>
    /// 泛型结果输出接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResultOutput<T> : IResultOutput
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        T Data { get; }
    }
}