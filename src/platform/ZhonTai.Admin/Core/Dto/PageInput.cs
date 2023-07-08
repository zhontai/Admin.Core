namespace ZhonTai.Admin.Core.Dto;

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
            if (_pageSize > 1000) _pageSize = 1000;
            return _pageSize;
        }
        set => _pageSize = value;
    }

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