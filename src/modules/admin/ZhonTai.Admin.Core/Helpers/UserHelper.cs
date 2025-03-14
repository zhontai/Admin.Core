﻿using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Resources;

namespace ZhonTai.Admin.Core.Helpers;

/// <summary>
/// 用户帮助类
/// </summary>
[InjectSingleton]
public class UserHelper(AdminCoreLocalizer adminCoreLocalizer)
{
    /// <summary>
    /// 检查密码
    /// </summary>
    /// <param name="password"></param>
    public void CheckPassword(string password)
    {
        if (!PasswordHelper.Verify(password))
        {
            throw ResultOutput.Exception(adminCoreLocalizer["密码为字母+数字+可选特殊字符，长度在6-16之间"]);
        }
    }
}