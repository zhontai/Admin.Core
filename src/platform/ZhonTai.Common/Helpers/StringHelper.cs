using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// 字符串帮助类
/// </summary>
public class StringHelper
{
    private static readonly char[] _constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    /// <summary>
    /// 生成随机字符串，默认32位
    /// </summary>
    /// <param name="length">随机数长度</param>
    /// <returns></returns>
    public static string GenerateRandom(int length = 32)
    {
        var newRandom = new StringBuilder();
        var rd = new Random();
        for (int i = 0; i < length; i++)
        {
            newRandom.Append(_constant[rd.Next(_constant.Length)]);
        }
        return newRandom.ToString();
    }

    /// <summary>
    /// 生成随机字符串，只包含数字
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GenerateRandomNumber(int length = 6)
    {
        var newRandom = new StringBuilder();
        var rd = new Random();
        for (int i = 0; i < length; i++)
        {
            newRandom.Append(_constant[rd.Next(10)]);
        }
        return newRandom.ToString();
    }

    public static string Format(string str, object obj)
    {
        if (str.IsNull())
        {
            return str;
        }
        string s = str;
        if (obj.GetType().Name == "JObject")
        {
            foreach (var item in (Newtonsoft.Json.Linq.JObject)obj)
            {
                var k = item.Key.ToString();
                var v = item.Value.ToString();
                s = Regex.Replace(s, "\\{" + k + "\\}", v, RegexOptions.IgnoreCase);
            }
        }
        else
        {
            foreach (System.Reflection.PropertyInfo p in obj.GetType().GetProperties())
            {
                var xx = p.Name;
                var yy = p.GetValue(obj).ToString();
                s = Regex.Replace(s, "\\{" + xx + "\\}", yy, RegexOptions.IgnoreCase);
            }
        }
        return s;
    }
}