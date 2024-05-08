using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.Encoders;
using System.Text;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// 提供基于SM3算法的哈希计算和HMAC功能。
/// </summary>
public static class SM3Encryption
{
    /// <summary>
    /// 计算给定数据的SM3哈希值（二进制表示）。
    /// </summary>
    /// <param name="data">要计算哈希的数据（字符串形式）。</param>
    /// <returns>SM3哈希值的二进制数组。</returns>
    public static byte[] ComputeSM3Hash(string data)
    {
        var msg = Encoding.UTF8.GetBytes(data); // 使用UTF8编码将字符串转换为字节数组
        var sm3 = new SM3Digest();
        sm3.BlockUpdate(msg, 0, msg.Length);
        var md = new byte[sm3.GetDigestSize()]; // SM3算法产生的哈希值大小
        sm3.DoFinal(md, 0);
        return md;
    }

    /// <summary>
    /// 计算给定数据的SM3哈希值（十六进制表示）。
    /// </summary>
    /// <param name="data">要计算哈希的数据（字符串形式）。</param>
    /// <returns>SM3哈希值的十六进制字符串。</returns>
    public static string ComputeSM3HashHex(string data)
    {
        return Hex.ToHexString(ComputeSM3Hash(data));
    }
    /// <summary>
    /// 计算给定数据的SM3哈希值（十六进制表示）。
    /// </summary>
    /// <param name="data">要计算哈希的数据（字符串形式）。</param>
    /// <returns>SM3哈希值的十六进制字符串。</returns>
    public static string ComputeSM3HashBase64(string data)
    {
        return Base64.ToBase64String(ComputeSM3Hash(data));
    }

    /// <summary>
    /// 计算给定数据和密钥的HMAC-SM3值（二进制表示）。
    /// </summary>
    /// <param name="data">要计算HMAC的数据（字符串形式）。</param>
    /// <param name="key">HMAC密钥（字符串形式）。</param>
    /// <returns>HMAC-SM3值的二进制数组。</returns>
    public static byte[] ComputeHMacSM3(string data, string key)
    {
        var msg = Encoding.UTF8.GetBytes(data);
        var keyBytes = Encoding.UTF8.GetBytes(key);

        var keyParameter = new KeyParameter(keyBytes);
        var sm3 = new SM3Digest();
        var mac = new HMac(sm3); // 带密钥的杂凑算法
        mac.Init(keyParameter);
        mac.BlockUpdate(msg, 0, msg.Length);
        var result = new byte[mac.GetMacSize()];
        mac.DoFinal(result, 0);
        return result;
    }

    /// <summary>
    /// 计算给定数据和密钥的HMAC-SM3值（十六进制表示）。
    /// </summary>
    /// <param name="data">要计算HMAC的数据（字符串形式）。</param>
    /// <param name="key">HMAC密钥（字符串形式）。</param>
    /// <returns>HMAC-SM3值的十六进制字符串。</returns>
    public static string ComputeHMACSM3Hex(string data, string key)
    {
        return Hex.ToHexString(ComputeHMacSM3(data, key));
    }
  /// <summary>
    /// 计算给定数据和密钥的HMAC-SM3值（Base64表示）。
    /// </summary>
    /// <param name="data">要计算HMAC的数据（字符串形式）。</param>
    /// <param name="key">HMAC密钥（字符串形式）。</param>
    /// <returns>HMAC-SM3值的Base64字符串。</returns>
    public static string ComputeHMACSM3Base64(string data, string key)
    {
        return Base64.ToBase64String(ComputeHMacSM3(data, key));
    }
}