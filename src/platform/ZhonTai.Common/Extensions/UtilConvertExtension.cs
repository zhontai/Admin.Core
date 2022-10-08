using System;
using System.Text;

namespace ZhonTai.Common.Extensions;

/// <summary>
/// 数据类型转换
/// </summary>
public static class UtilConvertExtension
{
    public static int ToInt(this object s, bool round = false)
    {
        if (s == null || s == DBNull.Value)
            return 0;

        if (s is bool b)
            return b ? 1 : 0;

        if (int.TryParse(s.ToString(), out int result))
            return result;

        if (s.GetType().IsEnum)
        {
            return (int)s;
        }

        var f = s.ToFloat();
        return round ? Convert.ToInt32(f) : (int)f;
    }

    public static long ToLong(this object s)
    {
        if (s == null || s == DBNull.Value)
            return 0L;

        long.TryParse(s.ToString(), out long result);
        return result;
    }

    public static double ToMoney(this object thisValue)
    {
        double reval;
        if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out reval))
        {
            return reval;
        }
        return 0;
    }

    public static double ToMoney(this object thisValue, double errorValue)
    {
        double reval;
        if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out reval))
        {
            return reval;
        }
        return errorValue;
    }

    public static string ToString(this object thisValue)
    {
        if (thisValue != null) return thisValue.ToString().Trim();
        return "";
    }

    public static string ToString(this object thisValue, string errorValue)
    {
        if (thisValue != null) return thisValue.ToString().Trim();
        return errorValue;
    }

    public static float ToFloat(this object s, int? digits = null)
    {
        if (s == null || s == DBNull.Value)
            return 0f;

        float.TryParse(s.ToString(), out float result);

        if (digits == null)
            return result;

        return (float)Math.Round(result, digits.Value);
    }

    public static double ToDouble(this object s, int? digits = null)
    {
        if (s == null || s == DBNull.Value)
            return 0d;

        double.TryParse(s.ToString(), out double result);

        if (digits == null)
            return result;

        return Math.Round(result, digits.Value);
    }

    public static decimal ToDecimal(this object thisValue)
    {
        decimal reval;
        if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out reval))
        {
            return reval;
        }
        return 0;
    }

    public static decimal ToDecimal(this object thisValue, decimal errorValue)
    {
        decimal reval;
        if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out reval))
        {
            return reval;
        }
        return errorValue;
    }

    public static DateTime ToDateTime(this object thisValue)
    {
        DateTime reval = DateTime.MinValue;
        if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out reval))
        {
            reval = Convert.ToDateTime(thisValue);
        }
        return reval;
    }

    public static DateTime ToDateTime(this object thisValue, DateTime errorValue)
    {
        DateTime reval;
        if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out reval))
        {
            return reval;
        }
        return errorValue;
    }

    public static DateTime ToDateTime(this long milliseconds)
    {
        return Extensions.DateTimeExtension.TimestampStart.AddMilliseconds(milliseconds);
    }

    public static bool ToBool(this object thisValue)
    {
        bool reval = false;
        if (thisValue != null && thisValue != DBNull.Value && bool.TryParse(thisValue.ToString(), out reval))
        {
            return reval;
        }
        return reval;
    }

    public static byte ToByte(this object s)
    {
        if (s == null || s == DBNull.Value)
            return 0;

        byte.TryParse(s.ToString(), out byte result);
        return result;
    }

    #region ==字节转换==

    /// <summary>
    /// 转换为16进制
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="lowerCase">是否小写</param>
    /// <returns></returns>
    public static string ToHex(this byte[] bytes, bool lowerCase = true)
    {
        if (bytes == null)
            return null;

        var result = new StringBuilder();
        var format = lowerCase ? "x2" : "X2";
        for (var i = 0; i < bytes.Length; i++)
        {
            result.Append(bytes[i].ToString(format));
        }

        return result.ToString();
    }

    /// <summary>
    /// 16进制转字节数组
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static byte[] HexToBytes(this string s)
    {
        if (s.IsNull())
            return null;
        var bytes = new byte[s.Length / 2];

        for (int x = 0; x < s.Length / 2; x++)
        {
            int i = (Convert.ToInt32(s.Substring(x * 2, 2), 16));
            bytes[x] = (byte)i;
        }

        return bytes;
    }

    /// <summary>
    /// 转换为Base64
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToBase64(this byte[] bytes)
    {
        if (bytes == null)
            return null;

        return Convert.ToBase64String(bytes);
    }

    #endregion ==字节转换==
}