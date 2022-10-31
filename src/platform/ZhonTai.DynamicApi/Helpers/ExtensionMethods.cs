using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ZhonTai.DynamicApi.Helpers;

internal static class ExtensionMethods
{
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    public static bool IsNullOrEmpty<T>(this ICollection<T> source)
    {
        return source == null || source.Count <= 0;
    }

    public static bool IsIn(this string str, params string[] data)
    {
        foreach (var item in data)
        {
            if (str == item)
            {
                return true;
            }
        }
        return false;
    }

    public static string RemovePostFix(this string str, params string[] postFixes)
    {
        if (str == null)
        {
            return null;
        }

        if (str == string.Empty)
        {
            return string.Empty;
        }

        if (postFixes.IsNullOrEmpty())
        {
            return str;
        }

        foreach (var postFix in postFixes)
        {
            if (str.EndsWith(postFix))
            {
                return str.Left(str.Length - postFix.Length);
            }
        }

        return str;
    }

    public static string RemovePreFix(this string str, params string[] preFixes)
    {
        if (str == null)
        {
            return null;
        }

        if (str == string.Empty)
        {
            return string.Empty;
        }

        if (preFixes.IsNullOrEmpty())
        {
            return str;
        }

        foreach (var preFix in preFixes)
        {
            if (str.StartsWith(preFix))
            {
                return str.Right(str.Length - preFix.Length);
            }
        }

        return str;
    }


    public static string Left(this string str, int len)
    {
        if (str == null)
        {
            throw new ArgumentNullException("str");
        }

        if (str.Length < len)
        {
            throw new ArgumentException("len argument can not be bigger than given string's length!");
        }

        return str.Substring(0, len);
    }


    public static string Right(this string str, int len)
    {
        if (str == null)
        {
            throw new ArgumentNullException("str");
        }

        if (str.Length < len)
        {
            throw new ArgumentException("len argument can not be bigger than given string's length!");
        }

        return str.Substring(str.Length - len, len);
    }

    public static string GetCamelCaseFirstWord(this string str)
    {
        if (str == null)
        {
            throw new ArgumentNullException(nameof(str));
        }

        if (str.Length == 1)
        {
            return str;
        }

        var res = Regex.Split(str, @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})");

        if (res.Length < 1)
        {
            return str;
        }
        else
        {
            return res[0];
        }
    }

    public static string GetPascalCaseFirstWord(this string str)
    {
        if (str == null)
        {
            throw new ArgumentNullException(nameof(str));
        }

        if (str.Length == 1)
        {
            return str;
        }

        var res = Regex.Split(str, @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})");

        if (res.Length < 2)
        {
            return str;
        }
        else
        {
            return res[1];
        }
    }

    public static string GetPascalOrCamelCaseFirstWord(this string str)
    {
        if (str == null)
        {
            throw new ArgumentNullException(nameof(str));
        }

        if (str.Length <= 1)
        {
            return str;
        }

        if (str[0] >= 65 && str[0] <= 90)
        {
            return GetPascalCaseFirstWord(str);
        }
        else
        {
            return GetCamelCaseFirstWord(str);
        }
    }

    public static string FirstCharToLower(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;

        string str = s.First().ToString().ToLower() + s.Substring(1);
        return str;
    }

    public static string FirstCharToUpper(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;

        string str = s.First().ToString().ToUpper() + s.Substring(1);
        return str;
    }
}