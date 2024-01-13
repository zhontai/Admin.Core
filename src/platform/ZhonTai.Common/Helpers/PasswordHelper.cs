using System.Text.RegularExpressions;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// 密码帮助类
/// </summary>
public partial class PasswordHelper
{
   
    // 验证密码的正则表达式  
    public static readonly string PasswordRegex = @"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d!@#$%^&.*]{6,16}$";

    /// <summary>  
    /// 验证密码是否符合要求  
    /// </summary>  
    /// <param name="input"></param>  
    /// <returns></returns>  
    public static bool Verify(string input)
    {
        if (input.IsNull())
        {
            return false;
        }

        return Regex.IsMatch(input, PasswordRegex);
    }
}