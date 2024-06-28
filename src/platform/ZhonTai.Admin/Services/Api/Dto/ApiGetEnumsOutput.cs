using System.Collections.Generic;

namespace ZhonTai.Admin.Services.Api.Dto;

/// <summary>
/// 枚举
/// </summary>
public class ApiGetEnumsOutput
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 选项列表
    /// </summary>
    public List<Models.Options> Options { get; set; }

    public static class Models
    {
        /// <summary>
        /// 选项
        /// </summary>
        public class Options
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// 值
            /// </summary>
            public long Value { get; set; }
        }
    }
}