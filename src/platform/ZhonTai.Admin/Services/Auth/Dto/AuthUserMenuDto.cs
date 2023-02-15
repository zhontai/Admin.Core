using Newtonsoft.Json;

namespace ZhonTai.Admin.Services.Auth.Dto;

public class AuthUserMenuDto
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
    /// 路由地址
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 路由命名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 视图地址
    /// </summary>
    public string ViewPath { get; set; }

    /// <summary>
    /// 重定向地址
    /// </summary>
    public string Redirect { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 打开
    /// </summary>
    public bool? Opened { get; set; }

    /// <summary>
    /// 隐藏
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// 打开新窗口
    /// </summary>
    public bool? NewWindow { get; set; }

    /// <summary>
    /// 链接外显
    /// </summary>
    public bool? External { get; set; }

    /// <summary>
    /// 是否缓存
    /// </summary>
    public bool IsKeepAlive { get; set; } = true;

    /// <summary>
    /// 是否固定
    /// </summary>
    public bool IsAffix { get; set; } = false;

    /// <summary>
    /// 链接地址
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// 是否内嵌窗口
    /// </summary>
    public bool IsIframe { get; set; } = false;

    /// <summary>
    /// 排序
    /// </summary>
    [JsonIgnore]
    public int? Sort { get; set; }
}