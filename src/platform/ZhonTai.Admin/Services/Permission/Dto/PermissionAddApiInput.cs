namespace ZhonTai.Admin.Services.Permission.Dto;

public class PermissionAddApiInput
{
    /// <summary>
    /// 父级节点
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 接口
    /// </summary>
    public long? ApiId { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 权限编码
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
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }
}