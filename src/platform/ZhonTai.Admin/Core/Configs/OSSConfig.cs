using OnceMi.AspNetCore.OSS;
using System.Collections.Generic;

namespace ZhonTai.Admin.Core.Configs;

/// <summary>
/// OSS配置
/// </summary>
public class OSSOptions
{
    /// <summary>
    /// 文件存储供应商
    /// </summary>
    public OSSProvider Provider { get; set; } = OSSProvider.Minio;
    /// <summary>
    /// 域名
    /// </summary>
    public string Endpoint { get; set; }
    /// <summary>
    /// 账号
    /// </summary>
    public string AccessKey { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string SecretKey { get; set; }
    /// <summary>
    /// 地区
    /// </summary>
    public string Region { get; set; }
    /// <summary>
    /// 会话Token
    /// </summary>
    public string SessionToken { get; set; }
    /// <summary>
    /// 启用Https
    /// </summary>
    public bool IsEnableHttps { get; set; }
    /// <summary>
    /// 启用缓存
    /// </summary>
    public bool IsEnableCache { get; set; }
    /// <summary>
    /// 存储桶
    /// </summary>
    public string BucketName { get; set; } = "admin";
    /// <summary>
    /// 文件地址
    /// </summary>
    public string Url { get; set; }
    /// <summary>
    /// 文件Md5码
    /// </summary>
    public bool Md5 { get; set; } = false;
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = false;
}

/// <summary>
/// 本地上传配置
/// </summary>
public class LocalUploadConfig
{
    /// <summary>
    /// 上传目录
    /// </summary>
    public string Directory { get; set; } = "upload";
    /// <summary>
    /// 日期目录
    /// </summary>
    public string DateTimeDirectory { get; set; } = "yyyy/MM/dd";
    /// <summary>
    /// 文件Md5码
    /// </summary>
    public bool Md5 { get; set; } = false;
}

/// <summary>
/// OSS配置
/// </summary>
public class OSSConfig
{
    /// <summary>
    /// 本地上传配置
    /// </summary>
    public LocalUploadConfig LocalUploadConfig { get; set; }

    /// <summary>
    /// 文件存储供应商
    /// </summary>
    public OSSProvider Provider { get; set; } = OSSProvider.Minio;

    /// <summary>
    /// OSS配置列表
    /// </summary>
    public List<OSSOptions> OSSConfigs { get; set; }
}