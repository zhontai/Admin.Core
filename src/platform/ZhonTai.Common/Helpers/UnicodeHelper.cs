using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ZhonTai.Common.Helpers;

public static class UnicodeHelper
{
    /// <summary>
    /// 字符串转Unicode码
    /// </summary>
    /// <returns>The to unicode.</returns>
    /// <param name="value">Value.</param>
    public static string StringToUnicode(string value)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(value);
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i += 2)
        {
            // 取两个字符，每个字符都是右对齐。
            stringBuilder.AppendFormat("u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Unicode转字符串
    /// </summary>
    /// <returns>The to string.</returns>
    /// <param name="unicode">Unicode.</param>
    public static string UnicodeToString(string unicode)
    {
        unicode = unicode.Replace("%", "\\");

        return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
             unicode, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
    }
}