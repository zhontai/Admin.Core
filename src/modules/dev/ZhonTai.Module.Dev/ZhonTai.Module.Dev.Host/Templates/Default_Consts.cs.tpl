@{
    var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    var moduleNamePc = gen.ApiAreaName?.NamingPascalCase();
}

using ZhonTai.Admin.Core.Attributes;
using System.ComponentModel;

namespace @(gen.Namespace).Core.Consts;

public static partial class @(moduleNamePc)Consts
{
    /// <summary>
    /// 默认域
    /// </summary>
    public const string AreaName = "@gen.ApiAreaName";

}

/// <summary>
/// 数据库键名
/// </summary>
public class DbKeys
{
    /// <summary>
    /// 数据库注册键
    /// </summary>
    [Description("数据库注册键")]
    public static string AppDb { get; set; } = "admindb";

}

/// <summary>
/// 缓存键
/// </summary>
[ScanCacheKeys]
public static partial class CacheKeys
{

}