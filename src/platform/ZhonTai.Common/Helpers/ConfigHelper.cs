using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// 配置帮助类
/// </summary>
public class ConfigHelper
{
    /* 使用热更新
    var uploadConfig = new ConfigHelper().Load("uploadconfig", _env.EnvironmentName, true);
    services.Configure<UploadConfig>(uploadConfig);

    private readonly UploadConfig _uploadConfig;
    public ImgController(IOptionsMonitor<UploadConfig> uploadConfig)
    {
        _uploadConfig = uploadConfig.CurrentValue;
    }
    */

    /// <summary>
    /// 加载配置文件
    /// </summary>
    /// <param name="fileName">文件名称</param>
    /// <param name="environmentName">环境名称</param>
    /// <param name="optional">可选</param>
    /// <param name="reloadOnChange">自动更新</param>
    /// <returns></returns>
    public static IConfiguration Load(string fileName, string environmentName = "", bool optional = true, bool reloadOnChange = false)
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Configs");
        if (!Directory.Exists(filePath))
            return null;

        var builder = new ConfigurationBuilder()
            .SetBasePath(filePath)
            .AddJsonFile(fileName.ToLower() + ".json", optional, reloadOnChange);

        if (environmentName.NotNull())
        {
            builder.AddJsonFile(fileName.ToLower() + "." + environmentName + ".json", optional: optional, reloadOnChange: reloadOnChange);
        }

        return builder.Build();
    }

    /// <summary>
    /// 获得配置信息
    /// </summary>
    /// <typeparam name="T">配置信息</typeparam>
    /// <param name="fileName">文件名称</param>
    /// <param name="environmentName">环境名称</param>
    /// <param name="optional">可选</param>
    /// <param name="reloadOnChange">自动更新</param>
    /// <returns></returns>
    public static T Get<T>(string fileName, string environmentName = "", bool optional = true, bool reloadOnChange = false)
    {
        var configuration = Load(fileName, environmentName, optional, reloadOnChange);
        if (configuration == null)
            return default;

        return configuration.Get<T>();
    }

    /// <summary>
    /// 绑定实例配置信息
    /// </summary>
    /// <param name="fileName">文件名称</param>
    /// <param name="instance">实例配置</param>
    /// <param name="environmentName">环境名称</param>
    /// <param name="optional">可选</param>
    /// <param name="reloadOnChange">自动更新</param>
    public static void Bind(string fileName, object instance, string environmentName = "", bool optional = true, bool reloadOnChange = false)
    {
        var configuration = Load(fileName, environmentName, optional, reloadOnChange);
        if (configuration == null || instance == null)
            return;

        configuration.Bind(instance);
    }
}