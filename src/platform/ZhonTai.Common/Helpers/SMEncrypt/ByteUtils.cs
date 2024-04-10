using System;
using System.Globalization;
using System.Text;

namespace ZhonTai.Common.Helpers.SMEncrypt;

/// <summary>
/// 字节数组操作扩展类
/// </summary>
static class ByteUtils
{
    internal static byte[] AsciiBytes(string s)
    {
        byte[] bytes = new byte[s.Length];

        for (int i = 0; i < s.Length; i++)
        {
            bytes[i] = (byte)s[i];
        }

        return bytes;
    }

    internal static byte[] HexToByteArray(this string hexString)
    {
        byte[] bytes = new byte[hexString.Length / 2];

        for (int i = 0; i < hexString.Length; i += 2)
        {
            string s = hexString.Substring(i, 2);
            bytes[i / 2] = byte.Parse(s, NumberStyles.HexNumber, null);
        }

        return bytes;
    }

    internal static string ByteArrayToHex(this byte[] bytes)
    {
        StringBuilder builder = new StringBuilder(bytes.Length * 2);

        foreach (byte b in bytes)
        {
            builder.Append(b.ToString("X2"));
        }

        return builder.ToString();
    }

    internal static string ByteArrayToHex(this byte[] bytes, int len)
    {
        return ByteArrayToHex(bytes).Substring(0, len * 2);
    }

    internal static byte[] RepeatByte(byte b, int count)
    {
        byte[] value = new byte[count];

        for (int i = 0; i < count; i++)
        {
            value[i] = b;
        }

        return value;
    }

    internal static byte[] SubBytes(this byte[] bytes, int startIndex, int length)
    {
        byte[] res = new byte[length];
        Array.Copy(bytes, startIndex, res, 0, length);
        return res;
    }

    internal static byte[] XOR(this byte[] value)
    {
        byte[] res = new byte[value.Length];
        for (int i = 0; i < value.Length; i++)
        {
            res[i] ^= value[i];
        }
        return res;
    }

    internal static byte[] XOR(this byte[] valueA, byte[] valueB)
    {
        int len = valueA.Length;
        byte[] res = new byte[len];
        for (int i = 0; i < len; i++)
        {
            res[i] = (byte)(valueA[i] ^ valueB[i]);
        }
        return res;
    }
}
