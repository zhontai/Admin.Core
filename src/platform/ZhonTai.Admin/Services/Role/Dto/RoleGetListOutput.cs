namespace ZhonTai.Admin.Services.Role.Dto;

public class RoleGetListOutput
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
}