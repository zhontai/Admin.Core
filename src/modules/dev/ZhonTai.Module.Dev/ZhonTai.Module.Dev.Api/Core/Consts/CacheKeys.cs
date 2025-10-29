using System.ComponentModel;

namespace ZhonTai.Module.Dev.Api.Core.Consts;

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

    /// <summary>
    /// 获取模块缓存键
    /// </summary>
    /// <param name="id">模块Id</param>
    /// <returns></returns>
    public static string GetModuleActionKey(long id) => $"{ModuleActionKey}{id}";
}