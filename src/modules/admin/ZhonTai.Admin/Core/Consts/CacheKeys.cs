using SixLabors.ImageSharp.Drawing;
using System.ComponentModel;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Core.Consts;

/// <summary>
/// 缓存键
/// </summary>
[ScanCacheKeys]
public static partial class CacheKeys
{
    /// <summary>
    /// 验证码 admin:captcha:guid
    /// </summary>
    [Description("验证码")]
    public const string Captcha = "admin:captcha:";

    /// <summary>
    /// 密码加密 admin:password:encrypt:guid
    /// </summary>
    [Description("密码加密")]
    public const string PassWordEncrypt = "admin:password:encrypt:";

    /// <summary>
    /// 用户权限 admin:user:permissions:userId
    /// </summary>
    [Description("用户权限")]
    public const string UserPermission = "admin:user:permission:";

    /// <summary>
    /// 数据权限 admin:user:data:permission:userId
    /// </summary>
    [Description("数据权限")]
    public const string DataPermission = "admin:user:data:permission:";

    /// <summary>
    /// 短信验证码 admin:sms:code:guid
    /// </summary>
    [Description("短信验证码")]
    public const string SmsCode = "admin:sms:code:";

    /// <summary>
    /// 邮箱验证码 admin:email:code:guid
    /// </summary>
    [Description("邮箱验证码")]
    public const string EmailCode = "admin:email:code:";

    /// <summary>
    /// 接口列表 admin:api:list
    /// </summary>
    [Description("接口列表")]
    public const string ApiList = "admin:api:list";

    /// <summary>
    /// Excel错误标记文件 admin:excel:error_mark:userId:fileId
    /// </summary>
    [Description("Excel错误标记文件")]
    public const string ExcelErrorMark = "admin:excel:error_mark:";

    /// <summary>
    /// 获取短信验证码缓存键
    /// </summary>
    /// <param name="mobile">手机号</param>
    /// <param name="code">唯一码</param>
    /// <returns></returns>
    public static string GetSmsCodeKey(string mobile, string code) => $"{SmsCode}{mobile}:{code}";

    /// <summary>
    /// 获取邮箱验证码缓存键
    /// </summary>
    /// <param name="email">邮件地址</param>
    /// <param name="code">唯一码</param>
    /// <returns></returns>
    public static string GetEmailCodeKey(string email, string code) => $"{EmailCode}{email}:{code}";

    /// <summary>
    /// 获取数据权限缓存键
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="apiPath">请求接口路径</param>
    /// <returns></returns>
    public static string GetDataPermissionKey(long userId, string apiPath = null)
    {
        if(apiPath.IsNull())
        {
            apiPath = AppInfo.CurrentDataPermissionApiPath;
        }

        return $"{DataPermission}{userId}{(apiPath.NotNull() ? (":" + apiPath) : "")}";
    }

    /// <summary>
    /// 获取数据权限缓存键
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <returns></returns>
    public static string GetUserPermissionKey(long userId) => $"{UserPermission}{userId}";

    /// <summary>
    /// 获取数据权限模板
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <returns></returns>
    public static string GetDataPermissionPattern(long userId) => $"{DataPermission}{userId}*";

    /// <summary>
    /// 获取Excel错误标记文件缓存键
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="fileId">文件Id</param>
    /// <returns></returns>
    public static string GetExcelErrorMarkKey(long userId, string fileId) => $"{ExcelErrorMark}{userId}{(fileId.NotNull() ? (":" + fileId) : "")}";
}