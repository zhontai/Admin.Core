namespace ZhonTai.Admin.Services.RoleGroup.Dto;

/// <summary>
/// 添加
/// </summary>
public class RoleGroupAddInput
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
	public int Sort { get; set; }
}