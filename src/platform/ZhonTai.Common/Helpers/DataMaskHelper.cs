using System.Text.RegularExpressions;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// 数据脱敏
/// </summary>
public partial class DataMaskHelper
{
    public static readonly string PhoneMaskRegex = @"(\d{3})\d{4}(\d{4})";
     
    public static readonly string EmailMaskRegex = "(?<=.{2})[^@]+(?=.{2}@)";

    public static readonly string IPMaskRegex = @"(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})";

    /// <summary>
    /// 手机号脱敏
    /// </summary>
    /// <param name="input"></param>
    /// <param name="mask"></param>
    /// <returns></returns>
    public static string PhoneMask(string input, string mask = "****")
    {
        if (input.IsNull()) { 
            return input; 
        }

        return Regex.Replace(input, PhoneMaskRegex, match => $"{match.Groups[1]?.Value}{mask}{match.Groups[2]?.Value}");
    }

    /// <summary>
    /// 邮箱脱敏
    /// </summary>
    /// <param name="input"></param>
    /// <param name="mask"></param>
    /// <returns></returns>
    public static string EmailMask(string input, string mask = "****")
    {
        if (input.IsNull())
        {
            return input;
        }

        return Regex.Replace(input, EmailMaskRegex, mask);
    }

    /// <summary>
    /// IP脱敏
    /// </summary>
    /// <param name="input"></param>
    /// <param name="mask"></param>
    /// <returns></returns>
    public static string IPMask(string input, string mask = "*")
    {
        if (input.IsNull())
        {
            return input;
        }

        return Regex.Replace(input, IPMaskRegex, match => $"{match.Groups[1]?.Value}.{mask}.{mask}.{match.Groups[4]?.Value}");
    }
}