namespace ZhonTai.Admin.Services.Permission.Dto;

public class PermissionAddGroupInput
{
    /// <summary>
    /// 父级节点
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 视图
    /// </summary>
    public long? ViewId { get; set; }

    /// <summary>
    /// 路由命名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 访问路由地址
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 重定向地址
    /// </summary>
    public string Redirect { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    public string Label { get; set; }

    ///// <summary>
    ///// 说明
    ///// </summary>
    //public string Description { get; set; }

    /// <summary>
    /// 隐藏
    /// </summary>
	public bool Hidden { get; set; } = false;

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 展开
    /// </summary>
    public bool Opened { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }
}