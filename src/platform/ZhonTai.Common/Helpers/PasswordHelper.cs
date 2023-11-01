using System.Text.RegularExpressions;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// 密码帮助类
/// </summary>
public partial class PasswordHelper
{
   
    [GeneratedRegex(@"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d!@#$%^&.*]{6,16}$")]
    public static partial Regex RegexPassword();

    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool Verify(string input)
    {
        if (input.IsNull()) { 
            return false; 
        }

        return RegexPassword().IsMatch(input);
    }
}