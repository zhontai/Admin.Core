using System;

namespace ZhonTai.Admin.Services.File.Dto;

public class FileGetPageOutput
{
    /// <summary>
    /// 文件Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// OSS供应商
    /// </summary>
    public string ProviderName { get; set; }

    /// <summary>
    /// 存储桶名称
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// 文件目录
    /// </summary>
    public string FileDirectory { get; set; }

    /// <summary>
    /// 文件Guid
    /// </summary>
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
    public string SizeFormat { get; set; }

    /// <summary>
    /// 链接地址
    /// </summary>
    public string LinkUrl { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 修改者
    /// </summary>
    public string ModifiedUserName { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModifiedTime { get; set; }
}