namespace ZhonTai.Admin.Core.Dto;

/// <summary>
/// 导入信息输出
/// </summary>
public class ImportOutput
{
    /// <summary>
    /// 数据总数
    /// </summary>
    public long Total { get; set; } = 0;

    /// <summary>
    /// 新增数
    /// </summary>
    public long InsertCount { get; set; } = 0;

    /// <summary>
    /// 更新数
    /// </summary>
    public long UpdateCount { get; set; } = 0;
}