using System.IO;
using System.Security.Cryptography;
using System.Text;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// MD5加密
/// </summary>
public class MD5Encrypt
{
    /// <summary>
    /// 16位MD5加密
    /// </summary>
    /// <param name="password"></param>
    /// <param name="lowerCase"></param>
    /// <returns></returns>
    public static string Encrypt16(string password, bool lowerCase = false)
    {
        if (password.IsNull())
            return null;

        using var md5 = MD5.Create();
        return md5.ComputeHash(Encoding.UTF8.GetBytes(password)).ToHex(lowerCase);
    }

    /// <summary>
    /// 32位MD5加密
    /// </summary>
    /// <param name="password"></param>
    /// <param name="lowerCase"></param>
    /// <returns></returns>
    public static string Encrypt32(string password = "", bool lowerCase = false)
    {
        if (password.IsNull())
            return null;

        using var md5 = MD5.Create();
        string pwd = string.Empty;
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        var format = lowerCase ? "x2" : "X2";
        foreach (var item in s)
        {
            pwd = string.Concat(pwd, item.ToString(format));
        }
        return pwd;
    }

    /// <summary>
    /// 64位MD5加密
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static string Encrypt64(string password)
    {
        if (password.IsNull())
            return null;

        using var md5 = MD5.Create();
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        return s.ToBase64();
    }

    public static string GetHash(Stream stream)
    {
        StringBuilder sb = new();
        using var md5 = MD5.Create();
        byte[] hashBytes = md5.ComputeHash(stream);
        foreach (byte bt in hashBytes)
        {
            sb.Append(bt.ToString("x2"));
        }

        return sb.ToString();
    }
}