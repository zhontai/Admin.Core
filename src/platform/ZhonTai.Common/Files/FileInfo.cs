namespace ZhonTai.Common.Files;

/// <summary>
/// 文件信息
/// </summary>
public class FileInfo
{
    public FileInfo()
    {
    }

    /// <summary>
    /// 初始化文件信息
    /// </summary>
    /// <param name="fileName">文件名称</param>
    /// <param name="size">大小</param>
    public FileInfo(string fileName, long size = 0L)
    {
        FileName = fileName;
        Size = new FileSize(size);
        Extension = System.IO.Path.GetExtension(FileName)?.TrimStart('.');
    }

    /// <summary>
    /// 上传路径
    /// </summary>
    public string UploadPath { get; set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    public string RequestPath { get; set; }

    /// <summary>
    /// 相对路径
    /// </summary>
    public string RelativePath { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 保存名
    /// </summary>
    public string SaveName { get; set; }

    /// <summary>
    /// 文件大小
    /// </summary>
    public FileSize Size { get; set; }

    /// <summary>
    /// 扩展名
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// 文件目录
    /// </summary>
    public string FileDirectory => System.IO.Path.Combine(UploadPath, RelativePath).ToPath();

    /// <summary>
    /// 文件请求路径
    /// </summary>
    public string FileRequestPath => System.IO.Path.Combine(RequestPath, RelativePath, SaveName).ToPath();

    /// <summary>
    /// 文件相对路径
    /// </summary>
    public string FileRelativePath => System.IO.Path.Combine(RelativePath, SaveName).ToPath();

    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath => System.IO.Path.Combine(UploadPath, RelativePath, SaveName).ToPath();
}