using System;
using System.ComponentModel;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Configs;

namespace ZhonTai.Module.Dev.Api.Core.Consts;

/// <summary>
/// 数据库键名
/// </summary>
public class DbKeys
{
    private static readonly string _defaultDbKey = AppInfo.GetOptions<DbConfig>()?.Key ?? "devdb";

    static DbKeys()
    {
        if (string.IsNullOrWhiteSpace(_defaultDbKey))
        {
            throw new InvalidOperationException("数据库配置键不能为空");
        }
    }


    /// <summary>
    /// 数据库注册键
    /// </summary>
    [Description("数据库注册键")]
    public static string AppDb { get; set; } = _defaultDbKey;
}