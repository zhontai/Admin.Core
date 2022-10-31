namespace ZhonTai.Admin.Domain.Role;

/// <summary>
/// 数据范围
/// </summary>
public enum DataScope
{
    /// <summary>
    /// 全部
    /// </summary>
    All = 1,

    /// <summary>
    /// 本部门和下级部门
    /// </summary>
    DeptWithChild = 2,

    /// <summary>
    /// 本部门
    /// </summary>
    Dept = 3,

    /// <summary>
    /// 本人数据
    /// </summary>
    Self = 4,

    /// <summary>
    /// 指定部门
    /// </summary>
    Custom = 5
}
