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
    public string PropName { get; set; }

    /// <summary>
    /// 排序方式
    /// </summary>
    public SortOrder? Order { get; set; }

    /// <summary>
    /// 是否升序
    /// </summary>
    public bool? IsAscending => Order.HasValue && Order.Value == SortOrder.Asc;
}