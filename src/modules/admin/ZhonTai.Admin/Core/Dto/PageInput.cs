using System.Collections.Generic;

namespace ZhonTai.Admin.Core.Dto;

/// <summary>
/// 排序方式
/// </summary>
public enum SortOrder
{
    Asc,
    Desc,
}

/// <summary>
/// 排序
/// </summary>
public class SortInput
{
    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropName {  get; set; }

    /// <summary>
    /// 排序方式
    /// </summary>
    public SortOrder? Order { get; set; }

    /// <summary>
    /// 是否升序
    /// </summary>
    public bool? IsAscending => Order.HasValue && Order.Value == SortOrder.Asc;
}

/// <summary>
/// 分页信息输入
/// </summary>
public class PageInput
{
    private int _currentPage;
    private int _pageSize;

    /// <summary>
    /// 当前页标
    /// </summary>
    public virtual int CurrentPage 
    {
        get => _currentPage < 1 ? 1 : _currentPage;
        set => _currentPage = value;
    }

    /// <summary>
    /// 每页大小
    /// </summary>
    public virtual int PageSize 
    {
        get
        {
            if (_pageSize < 1) _pageSize = 1;
            //if (_pageSize > 1000) _pageSize = 1000;
            return _pageSize;
        }
        set => _pageSize = value;
    }

    /// <summary>
    /// 高级查询条件
    /// </summary>
    public virtual FreeSql.Internal.Model.DynamicFilterInfo DynamicFilter { get; set; } = null;

    /// <summary>
    /// 排序列表
    /// </summary>
    public List<SortInput>? SortList { get; set; }
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