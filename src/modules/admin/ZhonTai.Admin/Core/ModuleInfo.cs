using System;
using System.Reflection;

namespace ZhonTai.Admin.Core;

/// <summary>
/// 模块信息
/// </summary>
public class ModuleInfo
{
    /// <summary>
    /// 任务层程序集
    /// </summary>
    public Assembly Assembly { get; set; }

    /// <summary>
    /// 多语言文件类型
    /// </summary>
    public Type LocalizerType { get; set; }
}
