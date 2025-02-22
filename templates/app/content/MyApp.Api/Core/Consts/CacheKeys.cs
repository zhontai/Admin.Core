using System.ComponentModel;

namespace MyApp.Api.Core.Consts;

/// <summary>
/// 缓存键
/// </summary>
public static partial class CacheKeys
{
    /// <summary>
    /// 模块缓存键 module:action:{id}
    /// </summary>
    [Description("模块缓存键")]
    public const string ModuleActionKey = "module:action:";
}