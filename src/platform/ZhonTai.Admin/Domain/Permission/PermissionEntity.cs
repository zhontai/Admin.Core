using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using System.Collections.Generic;
using ZhonTai.Admin.Domain.Api;
using ZhonTai.Admin.Domain.View;
using ZhonTai.Admin.Domain.PermissionApi;

namespace ZhonTai.Admin.Domain.Permission;

/// <summary>
/// 权限
/// </summary>
[Table(Name = "ad_permission")]
[Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Label), true)]
public partial class PermissionEntity : EntityBase
{
    /// <summary>
    /// 父级节点
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Label { get; set; }

    /// <summary>
    /// 权限编码
    /// </summary>
    [Column(StringLength = 500)]
    public string Code { get; set; }

    /// <summary>
    /// 权限类型
    /// </summary>
    [Column(MapType = typeof(int), CanUpdate = false)]
    public PermissionType Type { get; set; }

    /// <summary>
    /// 视图Id
    /// </summary>
    public long? ViewId { get; set; }

    /// <summary>
    /// 视图
    /// </summary>
    public ViewEntity View { get; set; }

    /// <summary>
    /// 路由命名
    /// </summary>
    [Column(StringLength = 100)]
    public string Name { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    [Column(StringLength = 500)]
    public string Path { get; set; }

    /// <summary>
    /// 重定向地址
    /// </summary>
    [Column(StringLength = 500)]
    public string Redirect { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [Column(StringLength = 100)]
    public string Icon { get; set; }

    /// <summary>
    /// 隐藏
    /// </summary>
	public bool Hidden { get; set; } = false;

    /// <summary>
    /// 展开分组
    /// </summary>
    public bool Opened { get; set; }

    /// <summary>
    /// 打开新窗口
    /// </summary>
    public bool NewWindow { get; set; } = false;

    /// <summary>
    /// 链接外显
    /// </summary>
    public bool External { get; set; } = false;

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
    [Column(StringLength = 500)]
    public string Link { get; set; }

    /// <summary>
    /// 是否内嵌窗口
    /// </summary>
    public bool IsIframe { get; set; } = false;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [Column(StringLength = 200)]
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    [Navigate(ManyToMany = typeof(PermissionApiEntity))]
    public ICollection<ApiEntity> Apis { get; set; }

    [Navigate(nameof(ParentId))]
    public List<PermissionEntity> Childs { get; set; }
}