using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileInfo = ZhonTai.Common.Files.FileInfo;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Dto;

namespace ZhonTai.Admin.Core.Helpers;

/// <summary>
/// 用户帮助类
/// </summary>
[SingleInstance]
public class UserHelper
{
    /// <summary>
    /// 检查密码
    /// </summary>
    /// <param name="password"></param>
    public void CheckPassword(string password)
    {
        if (!PasswordHelper.Verify(password))
        {
            throw ResultOutput.Exception("密码为字母+数字+可选特殊字符，长度在6-16之间");
        }
    }
}