namespace ZhonTai.Admin.Services.Role.Dto;

/// <summary>
/// 角色分页响应
/// </summary>
public class RoleGetPageOutput
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 隐藏
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}