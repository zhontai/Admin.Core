using System;
using System.IO;

namespace ZhonTai.Admin.Core.Configs;

/// <summary>
/// 上传配置
/// </summary>
public class UploadConfig
{
    /// <summary>
    /// 头像上传配置
    /// </summary>
    public FileUploadConfig Avatar { get; set; }

    /// <summary>
    /// 文档图片上传配置
    /// </summary>
    public FileUploadConfig Document { get; set; }
}

/// <summary>
/// 文件上传配置
/// </summary>
public class FileUploadConfig
{
    private string _uploadPath;

    /// <summary>
    /// 上传路径
    /// </summary>
    public string UploadPath
    {
        get
        {
            if (_uploadPath.IsNull())
            {
                _uploadPath = Path.Combine(AppContext.BaseDirectory, "upload").ToPath();
            }

            if (!Path.IsPathRooted(_uploadPath))
            {
                _uploadPath = Path.Combine(AppContext.BaseDirectory, _uploadPath).ToPath();
            }

            return _uploadPath;
        }
        set => _uploadPath = value;
    }

    /// <summary>
    /// 请求路径
    /// </summary>
    public string RequestPath { get; set; }

    /// <summary>
    /// 路径格式
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    /// 路径日期格式
    /// </summary>
    public string DateTimeFormat { get; set; }

    /// <summary>
    /// 文件大小 10M = 10 * 1024 * 1024
    /// </summary>
    public long MaxSize { get; set; }

    /// <summary>
    /// 最大允许上传个数 -1不限制
    /// </summary>
    public int Limit { get; set; } = -1;

    /// <summary>
    /// 文件格式
    /// </summary>
    public string[] ContentType { get; set; }
}