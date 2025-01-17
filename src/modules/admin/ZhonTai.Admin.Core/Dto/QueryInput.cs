using FreeSql.Internal.Model;
using System.Collections.Generic;

namespace ZhonTai.Admin.Core.Dto;

/// <summary>
/// 查询信息输入
/// </summary>
public abstract class QueryInput
{
    /// <summary>
    /// 高级查询条件
    /// </summary>
    public virtual DynamicFilterInfo DynamicFilter { get; set; } = null;

    /// <summary>
    /// 排序列表
    /// </summary>
    public virtual List<SortInput>? SortList { get; set; }
}