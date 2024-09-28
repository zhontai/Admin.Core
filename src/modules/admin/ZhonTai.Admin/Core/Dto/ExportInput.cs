namespace ZhonTai.Admin.Core.Dto;

/// <summary>
/// 导出信息输入
/// </summary>
public class ExportInput: QueryInput
{
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }
}

/// <summary>
/// 导出信息输入
/// </summary>
/// <typeparam name="T">过滤数据</typeparam>
public class ExportInput<T>: ExportInput
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public virtual T Filter { get; set; }
}