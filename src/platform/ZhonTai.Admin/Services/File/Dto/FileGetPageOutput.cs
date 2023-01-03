using OnceMi.AspNetCore.OSS;
using System;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Services.File.Dto;

public class FileGetPageOutput
{
    /// <summary>
    /// OSS供应商
    /// </summary>
    public OSSProvider? Provider { get; set; }

    /// <summary>
    /// 存储桶名称
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// 文件Guid
    /// </summary>
    [OrderGuid]
    public Guid FileGuid { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// 文件大小格式化
    /// </summary>
    public string SizeFormat { get; }

    /// <summary>
    /// 链接地址
    /// </summary>
    public string LinkUrl { get; }
}