using OnceMi.AspNetCore.OSS;
using System.Collections.Generic;

namespace ZhonTai.Admin.Core.Configs;

public class OSSOptions
{
    public OSSProvider Provider { get; set; } = OSSProvider.Minio;
    public string Endpoint { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string Region { get; set; }
    public string SessionToken { get; set; }
    public bool IsEnableHttps { get; set; } = true;
    public bool IsEnableCache { get; set; }
    public string BucketName { get; set; } = "admin";
    public string Url { get; set; }
    public bool Enable { get; set; } = false;
}

/// <summary>
/// OSS配置
/// </summary>
public class OSSConfig
{
    public OSSProvider Provider { get; set; } = OSSProvider.Minio;

    public List<OSSOptions> OSSConfigs { get; set; }
}