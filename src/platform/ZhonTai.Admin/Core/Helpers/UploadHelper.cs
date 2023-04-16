using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileInfo = ZhonTai.Common.Files.FileInfo;
using Yitter.IdGenerator;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Configs;

namespace ZhonTai.Admin.Core.Helpers;

/// <summary>
/// 文件上传帮助类
/// </summary>
[SingleInstance]
public class UploadHelper
{
    /// <summary>
    /// 上传单文件
    /// </summary>
    /// <param name="file"></param>
    /// <param name="config"></param>
    /// <param name="args"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<FileInfo> UploadAsync(IFormFile file, FileUploadConfig config, object args, CancellationToken cancellationToken = default)
    { 
        if (file == null || file.Length < 1)
        {
            throw ResultOutput.Exception("请上传文件");
        }

        //格式限制
        if (!config.ContentType.Contains(file.ContentType))
        {
            throw ResultOutput.Exception("文件格式错误");
        }

        //大小限制
        if (!(file.Length <= config.MaxSize))
        {
            throw ResultOutput.Exception("文件过大");
        }

        var fileInfo = new FileInfo(file.FileName, file.Length)
        {
            UploadPath = config.UploadPath,
            RequestPath = config.RequestPath
        };

        var dateTimeFormat = config.DateTimeFormat.NotNull() ? DateTime.Now.ToString(config.DateTimeFormat) : "";
        var format = config.Format.NotNull() ? StringHelper.Format(config.Format, args) : "";
        fileInfo.RelativePath = Path.Combine(dateTimeFormat, format).ToPath();

        if (!Directory.Exists(fileInfo.FileDirectory))
        {
            Directory.CreateDirectory(fileInfo.FileDirectory);
        }

        fileInfo.SaveName = $"{YitIdHelper.NextId()}.{fileInfo.Extension}";

        await SaveAsync(file, fileInfo.FilePath, cancellationToken);

        return fileInfo;
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="file"></param>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SaveAsync(IFormFile file, string filePath, CancellationToken cancellationToken = default)
    {
        using var stream = File.Create(filePath);
        await file.CopyToAsync(stream, cancellationToken);
    }
}