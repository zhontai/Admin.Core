namespace ZhonTai.Admin.Core.Dto;

/// <summary>
/// 分页信息输入
/// </summary>
public class PageInput
{
    /// <summary>
    /// 当前页标
    /// </summary>
    public virtual int CurrentPage { get; set; } = 1;

    /// <summary>
    /// 每页大小
    /// </summary>
    public virtual int PageSize { set; get; } = 50;

    /// <summary>
    /// 高级查询条件
    /// </summary>
    public virtual FreeSql.Internal.Model.DynamicFilterInfo DynamicFilter { get; set; } = null;
}

/// <summary>
/// 分页信息输入
/// </summary>
/// <typeparam name="T">过滤数据</typeparam>
public class PageInput<T>: PageInput
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public virtual T Filter { get; set; }
}