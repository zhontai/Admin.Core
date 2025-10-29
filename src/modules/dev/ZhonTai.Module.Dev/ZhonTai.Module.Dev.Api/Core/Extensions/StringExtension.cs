using System;

public static class StringExtension
{
    /// <summary>
    /// 如果不是以指定的字符串结尾，则加入
    /// </summary>
    /// <param name="str"></param>
    /// <param name="endWith"></param>
    /// <returns></returns>
    /// <remarks>区分大小写</remarks>
    public static string PadEndIfNot(this string str, string endWith)
    {
        if (!str.EndsWith(endWith))
            return str + endWith;
        return str;
    }
    public static string PadEndIfNot(this string str,bool contion,string endWith)
    {
        if(contion)
            return str.PadEndIfNot(endWith);
        return str;
    }
    /// <summary>
    /// 如果指定字符串不为空，则所加到目标结尾
    /// </summary>
    /// <param name="str"></param>
    /// <param name="toConcat"></param>
    /// <returns></returns>
    public static string PadEndIfNotEmpty(this string str, string? toConcat)
    {
        if(!String.IsNullOrWhiteSpace(toConcat))
            str= str + toConcat;
        return str;
    }
    /// <summary>
    /// 如果指定字符串不为空，则所加另一个字符串到目标结尾
    /// </summary>
    /// <param name="str"></param>
    /// <param name="checking"></param>
    /// <param name="toConcat"></param>
    /// <returns></returns>
    public static string PadEndIfNotEmpty(this string str,string? checking,string toConcat)
    {
        if (!string.IsNullOrWhiteSpace(checking))
            str = str + toConcat;
        return str;
    }

    /// <summary>
    /// 用 新字符 替换指定指定内容中的每一个字符
    /// </summary>
    /// <param name="str"></param>
    /// <param name="oldChars"></param>
    /// <param name="newChar"></param>
    /// <returns></returns>
    public static string Replace(this string str, string oldChars, char newChar)
    {
        return str.Replace(oldChars.ToCharArray(), newChar);
    }
    /// <summary>
    /// 用 新字符 替换字符列表中的每个字符
    /// </summary>
    /// <param name="str"></param>
    /// <param name="oldChars"></param>
    /// <param name="newChar"></param>
    /// <returns></returns>
    public static string Replace(this string str, char[] oldChars, char newChar)
    {
        foreach (var c in oldChars)
        {
            str = str.Replace(c, newChar);
        }
        return str;
    }

}
