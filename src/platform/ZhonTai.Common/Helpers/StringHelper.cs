using System.Security.Cryptography;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// 字符串帮助类
/// </summary>
public partial class StringHelper
{
    private static readonly string _chars = "0123456789";
    private static readonly char[] _constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };


    /// <summary>
    /// 生成加密安全随机字符串，默认32位（使用 RandomNumberGenerator）
    /// </summary>
    /// <param name="length">随机数长度</param>
    /// <returns></returns>
    public static string GenerateRandom(int length = 32)
    {
        var data = new byte[length * 4];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(data);

        var result = new char[length];
        for (int i = 0; i < length; i++)
        {
            // 每次取4个字节转为无偏索引，避免取模偏差
            uint value = BitConverter.ToUInt32(data, i * 4);
            result[i] = _constant[(int)(value % _constant.Length)];
        }
        return new string(result);
    }

    /// <summary>
    /// 生成加密安全随机数字串，默认6位（使用 RandomNumberGenerator）
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GenerateRandomNumber(int length = 6)
    {
        var data = new byte[length * 4];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(data);

        var result = new char[length];
        for (int i = 0; i < length; i++)
        {
            uint value = BitConverter.ToUInt32(data, i * 4);
            result[i] = _chars[(int)(value % _chars.Length)];
        }
        return new string(result);
    }
}