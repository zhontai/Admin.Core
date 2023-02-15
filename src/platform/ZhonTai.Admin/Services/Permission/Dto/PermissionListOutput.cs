using ZhonTai.Admin.Domain.Permission;

namespace ZhonTai.Admin.Services.Permission.Dto;

public class PermissionListOutput
{
    /// <summary>
    /// 权限Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 父级节点
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 权限类型
    /// </summary>
    public PermissionType Type { get; set; }

    /// <summary>
    ///路由地址
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 重定向地址
    /// </summary>
    public string Redirect { get; set; }

    /// <summary>
    /// 视图地址
    /// </summary>
    public string ViewPath { get; set; }

    /// <summary>
    /// 链接地址
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// 接口路径
    /// </summary>
    public string ApiPaths { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 展开
    /// </summary>
    public bool Opened { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; }
}