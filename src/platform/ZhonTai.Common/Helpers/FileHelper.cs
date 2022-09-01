using System;
using System.IO;
using System.Text;

namespace ZhonTai.Common.Helpers;

public class FileHelper : IDisposable
{
    private bool _alreadyDispose = false;

    public FileHelper()
    {
    }

    ~FileHelper()
    {
        Dispose();
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (_alreadyDispose) return;
        _alreadyDispose = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #region 写文件

    /// <summary>
    /// 写文件
    /// </summary>
    /// <param name="Path">文件路径</param>
    /// <param name="Strings">文件内容</param>
    public static void WriteFile(string Path, string Strings)
    {
        if (!File.Exists(Path))
        {
            File.Create(Path).Close();
        }
        var streamWriter = new StreamWriter(Path, false);
        streamWriter.Write(Strings);
        streamWriter.Close();
        streamWriter.Dispose();
    }

    /// <summary>
    /// 写文件
    /// </summary>
    /// <param name="Path">文件路径</param>
    /// <param name="Strings">文件内容</param>
    /// <param name="encode">编码格式</param>
    public static void WriteFile(string Path, string Strings, Encoding encode)
    {
        if (!File.Exists(Path))
        {
            File.Create(Path).Close();
        }
        var streamWriter = new StreamWriter(Path, false, encode);
        streamWriter.Write(Strings);
        streamWriter.Close();
        streamWriter.Dispose();
    }

    #endregion 写文件

    #region 读文件

    /// <summary>
    /// 读文件
    /// </summary>
    /// <param name="Path">文件路径</param>
    /// <returns></returns>
    public static string ReadFile(string Path)
    {
        string s;
        if (!File.Exists(Path))
            s = "不存在相应的目录";
        else
        {
            var streamReader = new StreamReader(Path);
            s = streamReader.ReadToEnd();
            streamReader.Close();
            streamReader.Dispose();
        }

        return s;
    }

    /// <summary>
    /// 读文件
    /// </summary>
    /// <param name="Path">文件路径</param>
    /// <param name="encode">编码格式</param>
    /// <returns></returns>
    public static string ReadFile(string Path, Encoding encode)
    {
        string s;
        if (!File.Exists(Path))
            s = "不存在相应的目录";
        else
        {
            var streamReader = new StreamReader(Path, encode);
            s = streamReader.ReadToEnd();
            streamReader.Close();
            streamReader.Dispose();
        }

        return s;
    }

    #endregion 读文件
}