using System.Collections.Generic;

namespace My.Admin.Common.Output
{
    /// <summary>
    /// 分页信息输出
    /// </summary>
    public class PageOutput<T>
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public long Total { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public IList<T> List { get; set; }
    }
}