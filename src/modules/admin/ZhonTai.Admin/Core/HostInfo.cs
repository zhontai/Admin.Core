using System;
using System.Reflection;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Admin.Core;

/// <summary>
/// 宿主信息
/// </summary>
public class HostInfo
{
    /// <summary>
    /// 唯一Id
    /// </summary>
    public string Id { get; private set; } = string.Empty;

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// 短命名
    /// </summary>
    public string ShortName { get; private set; } = string.Empty;

    /// <summary>
    /// 全命名
    /// </summary>
    public string FullName { get; private set; } = string.Empty;

    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; private set; } = string.Empty;

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    private HostInfo()
    {
    }

    public static HostInfo CreateInstance(Assembly assembly)
    {
        if (assembly is null)
            assembly = Assembly.GetEntryAssembly() ?? throw new NullReferenceException(nameof(assembly));

        var attribute = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
        var description = attribute is null ? string.Empty : attribute.Description;
        var version = assembly.GetName().Version ?? throw new NullReferenceException("startAssembly.GetName().Version");
        var assemblyName = assembly.GetName().Name ?? string.Empty;
        var serviceName = assemblyName.Replace(".", "-").ToLower();
        var ticks = DateTime.Now.ToTimestamp(true);
        var ticksHex = Convert.ToString(ticks, 16);
        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();
        var serviceId = envName switch
        {
            "development" => $"{serviceName}-dev-{ticksHex}",
            "test" => $"{serviceName}-test-{ticksHex}",
            "staging" => $"{serviceName}-stag-{ticksHex}",
            "production" => $"{serviceName}-prod-{ticksHex}",
            _ => $"{serviceName}-{envName}-{ticksHex}",
        };

        var assemblyNames = assemblyName.Split(".");

        return new HostInfo
        {
            Id = serviceId,
            Name = assemblyNames[^2].ToLower(),
            FullName = serviceName,
            ShortName = $"{assemblyNames[^2]}-{assemblyNames[^1]}".ToLower(),
            Description = description,
            Version = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}"
        };
    }
}
