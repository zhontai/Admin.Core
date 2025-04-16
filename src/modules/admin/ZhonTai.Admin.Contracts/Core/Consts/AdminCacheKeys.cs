using System.ComponentModel;

namespace ZhonTai.Admin.Contracts.Core.Consts;

/// <summary>
/// 缓存键
/// </summary>
public static partial class AdminCacheKeys
{
    /// <summary>
    /// 部门 admin:org:tenantId
    /// </summary>
    [Description("部门")]
    public const string Org = "admin:org:";

    /// <summary>
    /// 获取部门缓存键
    /// </summary>
    /// <param name="tenantId">租户Id</param>
    /// <returns></returns>
    public static string GetOrgKey(long tenantId) => $"{Org}{tenantId}";
}