using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhonTai.Admin.Core.Dto;

/// <summary>
/// 加密传输时使用参数
/// </summary>
public class QueryInput
{
    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 传输的加密后信息
    /// </summary>
    public string Message { get; set; }
}

